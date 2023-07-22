using AdobeScriptMaker.UI.Core.Timeline;

namespace AdobeScriptMaker.UI.Core.Tests.ViewModels
{
    public class TimelineViewModelTests
    {
        [Test]
        public void Start_Can_Not_Be_Less_Than_Zero()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var componentVm = new TimelineComponentViewModel() { Start = 0, End = 100 };
            mainVm.Components.Add(componentVm);

            mainVm.Receive(new ResizeTimelineComponentMessage(componentVm, -1, ResizeDirection.Start));

            Assert.AreEqual(0, componentVm.Start);
        }

        [Test]
        public void Start_Can_Not_Be_Less_Than_Previous_Component_End()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var firstComponent = new TimelineComponentViewModel() { Start = 0, End = 100 };
            var secondComponent = new TimelineComponentViewModel() { Start = 100, End = 200 };

            mainVm.Components.Add(firstComponent);
            mainVm.Components.Add(secondComponent);

            mainVm.Receive(new ResizeTimelineComponentMessage(secondComponent, -1, ResizeDirection.Start));

            Assert.AreEqual(100, secondComponent.Start);
        }

        [Test]
        public void Start_Can_Decrease_If_There_Is_Space()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var firstComponent = new TimelineComponentViewModel() { Start = 0, End = 100 };
            var secondComponent = new TimelineComponentViewModel() { Start = 150, End = 200 };

            mainVm.Components.Add(firstComponent);
            mainVm.Components.Add(secondComponent);

            mainVm.Receive(new ResizeTimelineComponentMessage(secondComponent, -25, ResizeDirection.Start));

            Assert.AreEqual(125, secondComponent.Start);
        }

        [Test]
        public void Start_Can_Increase_If_It_Is_Less_Than_End()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var componentVm = new TimelineComponentViewModel() { Start = 50, End = 100 };
            mainVm.Components.Add(componentVm);

            mainVm.Receive(new ResizeTimelineComponentMessage(componentVm, 25, ResizeDirection.Start));

            Assert.AreEqual(75, componentVm.Start);
        }

        [Test]
        public void Start_Can_Not_Increase_Past_End()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var componentVm = new TimelineComponentViewModel() { Start = 50, End = 100 };
            mainVm.Components.Add(componentVm);

            mainVm.Receive(new ResizeTimelineComponentMessage(componentVm, 55, ResizeDirection.Start));

            Assert.AreEqual(50, componentVm.Start);
        }

        [Test]
        public void End_Can_Not_Be_Greater_Than_The_Width_Of_The_Timeline()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var componentVm = new TimelineComponentViewModel() { Start = 0, End = 100 };
            mainVm.Components.Add(componentVm);

            mainVm.Receive(new ResizeTimelineComponentMessage(componentVm, 500, ResizeDirection.End));

            Assert.AreEqual(100, componentVm.End);
        }

        [Test]
        public void End_Can_Not_Be_Greater_Than_Next_Component_Start()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var firstComponent = new TimelineComponentViewModel() { Start = 0, End = 100 };
            var secondComponent = new TimelineComponentViewModel() { Start = 100, End = 200 };

            mainVm.Components.Add(firstComponent);
            mainVm.Components.Add(secondComponent);

            mainVm.Receive(new ResizeTimelineComponentMessage(firstComponent, 1, ResizeDirection.End));

            Assert.AreEqual(100, firstComponent.End);
        }

        [Test]
        public void End_Can_Increase_If_There_Is_Space()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var firstComponent = new TimelineComponentViewModel() { Start = 0, End = 100 };
            var secondComponent = new TimelineComponentViewModel() { Start = 150, End = 200 };

            mainVm.Components.Add(firstComponent);
            mainVm.Components.Add(secondComponent);

            mainVm.Receive(new ResizeTimelineComponentMessage(firstComponent, 25, ResizeDirection.End));

            Assert.AreEqual(125, firstComponent.End);
        }

        [Test]
        public void End_Can_Decrease_If_It_Is_Greater_Than_Start()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var componentVm = new TimelineComponentViewModel() { Start = 50, End = 100 };
            mainVm.Components.Add(componentVm);

            mainVm.Receive(new ResizeTimelineComponentMessage(componentVm, -25, ResizeDirection.End));

            Assert.AreEqual(75, componentVm.End);
        }

        [Test]
        public void End_Can_Not_Decrease_Below_Start()
        {
            var mainVm = new TimelineViewModel() { Width = 500 };

            var componentVm = new TimelineComponentViewModel() { Start = 50, End = 100 };
            mainVm.Components.Add(componentVm);

            mainVm.Receive(new ResizeTimelineComponentMessage(componentVm, -55, ResizeDirection.End));

            Assert.AreEqual(100, componentVm.End);
        }
    }
}