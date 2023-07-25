using AdobeScriptMaker.Core.ComponentsConverters;
using AdobeScriptMaker.Core;
using AdobeScriptMaker.UI.Core.MainWindows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot;
using RenderingDescriptions.Timing;
using RenderingDescriptions.When;
using RenderingDescriptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public partial class TimelineViewModel : ObservableRecipient,
        IRecipient<ResizeTimelineComponentMessage>,
        IRecipient<RepositionTimelineComponentMessage>,
        IRecipient<AddTimelineComponentMessage>,
        IRecipient<GenerateScriptMessage>,
        IRecipient<InitializeStateMessage>
    {
        [ObservableProperty]
        private ObservableCollection<TimelineComponentViewModel> components = new ObservableCollection<TimelineComponentViewModel>();

        [ObservableProperty]
        private double width;

        private double _position;
        public double Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));

                    WeakReferenceMessenger.Default.Send(
                        new TimelinePositionUpdatedMessage(_position, Components.Where(x => x.Start <= _position && _position <= x.End)));
                }
            }
        }

        [RelayCommand]
        private void UpdatePosition(object arg)
        {
            var change = (double)arg;
            Position += change;
        }

        public TimelineViewModel()
        {
            WeakReferenceMessenger.Default.Register<ResizeTimelineComponentMessage>(this);
            WeakReferenceMessenger.Default.Register<RepositionTimelineComponentMessage>(this);
            WeakReferenceMessenger.Default.Register<AddTimelineComponentMessage>(this);
            WeakReferenceMessenger.Default.Register<GenerateScriptMessage>(this);
            WeakReferenceMessenger.Default.Register<InitializeStateMessage>(this);
        }

        public void Receive(ResizeTimelineComponentMessage message)
        {
            if (message.Direction == ResizeDirection.End)
            {
                var endBounds = GetEndMovementBounds(message.Component);
                message.Component.End = endBounds.ClampedValue(message.Component.End + message.SizeChange);
            }
            else if (message.Direction == ResizeDirection.Start)
            {
                var startBounds = GetStartMovementBounds(message.Component);
                message.Component.Start = startBounds.ClampedValue(message.Component.Start + message.SizeChange);
            }

            WeakReferenceMessenger.Default.Send(
                        new TimelinePositionUpdatedMessage(_position, Components.Where(x => x.Start <= _position && _position <= x.End)));
        }

        public void Receive(RepositionTimelineComponentMessage message)
        {
            var startBounds = GetStartMovementBounds(message.Component);
            var usedStartDist = startBounds.GetUsedDistance(message.Component.Start, message.PositionChange);

            var endBounds = GetEndMovementBounds(message.Component);
            var usedEndDist = endBounds.GetUsedDistance(message.Component.End, message.PositionChange);

            var minDist = Math.Min(usedStartDist, usedEndDist);
            var updatedChange = Math.Sign(message.PositionChange) * minDist;

            message.Component.Start = message.Component.Start + updatedChange;
            message.Component.End = message.Component.End + updatedChange;

            WeakReferenceMessenger.Default.Send(
                        new TimelinePositionUpdatedMessage(_position, Components.Where(x => x.Start <= _position && _position <= x.End)));
        }

        public void Receive(AddTimelineComponentMessage message)
        {
            double start;
            if (!Components.Any())
                start = 0;
            else
                start = Components.Max(x => x.End);

            Components.Add(new TimelineComponentViewModel() { WrappedComponent = message.Component, Name = message.Component.Name, Start = start, End = start + 100 });

            WeakReferenceMessenger.Default.Send(
                        new TimelinePositionUpdatedMessage(_position, Components.Where(x => x.Start <= _position && _position <= x.End)));
        }

        public void Receive(GenerateScriptMessage message)
        {
            foreach (var component in Components)
            {
                if (component.Name == "Plot Axes")
                {
                    var plotLayoutDescription = new PlotLayoutDescription(
                        new PlotAxesLayoutDescription(
                        new PlotAxisLayoutDescription(690, 0, 5),
                        new PlotAxisLayoutDescription(690, 0, 5)), new PointF(100, 300));

                    var axes = new AxesRenderingDescription("Axes",
                        plotLayoutDescription);

                    var axesToRender = new RenderingDescription(axes, new TimingForRender(new AbsoluteTiming(0), new AbsoluteTiming(Width)) { EntranceAnimationDuration = new AbsoluteTiming(0.5) }, null);

                    var converter = new UpdatedComponentsConverter();
                    var converted = converter.Convert(new List<RenderingDescription>() { axesToRender });

                    var scriptCreator = new ComponentsScriptCreator();
                    var script = scriptCreator.Visit(converted);
                }
            }
        }

        public void Receive(InitializeStateMessage message)
        {
            Position = message.Position;
            Width = message.Width;
        }

        private MovementBounds GetEndMovementBounds(TimelineComponentViewModel component)
        {
            return new MovementBounds(component.Start, GetEndMax(component));
        }

        private MovementBounds GetStartMovementBounds(TimelineComponentViewModel component)
        {
            return new MovementBounds(GetStartMin(component), component.End);
        }

        private double GetEndMax(TimelineComponentViewModel component)
        {
            var index = components.IndexOf(component);
            if (index == components.Count - 1)
                return width;
            else
            {
                var nextComponent = components[index + 1];
                return nextComponent.Start;
            }
        }

        private double GetStartMin(TimelineComponentViewModel component)
        {
            var index = components.IndexOf(component);
            if (index == 0)
                return 0;
            else
            {
                var previousComponent = components[index - 1];
                return previousComponent.End;
            }
        }

        private class MovementBounds
        {
            private readonly double _min;
            private readonly double _max;

            public MovementBounds(double min, double max)
            {
                _min = min;
                _max = max;
            }

            public bool IsInBounds(double value)
            {
                return _min <= value && value <= _max;
            }

            public double ClampedValue(double value)
            {
                if (value < _min)
                    return _min;
                else if (value > _max)
                    return _max;
                else
                    return value;
            }

            public double GetUsedDistance(double originalValue, double additionalValue)
            {
                var updatedValue = originalValue + additionalValue;
                if (updatedValue < _min)
                    return originalValue - _min;
                else if (updatedValue > _max)
                    return _max - originalValue;
                else
                    return Math.Abs(additionalValue);
            }
        }
    }
}
