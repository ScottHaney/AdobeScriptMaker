using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public partial class TimelineViewModel : ObservableRecipient, IRecipient<ResizeTimelineComponentMessage>, IRecipient<RepositionTimelineComponentMessage>
    {
        [ObservableProperty]
        private ObservableCollection<TimelineComponentViewModel> components = new ObservableCollection<TimelineComponentViewModel>();

        [ObservableProperty]
        private double width;

        public TimelineViewModel()
        {
            WeakReferenceMessenger.Default.Register<ResizeTimelineComponentMessage>(this);
            WeakReferenceMessenger.Default.Register<RepositionTimelineComponentMessage>(this);
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
