using AdobeScriptMaker.UI.Core.Timeline;

namespace AdobeScriptMaker.UI.Core.Tests.ViewModels
{
    public class TimelineViewModelTests
    {
        [Test]
        public void First_Component_Start_Cant_Go_Less_Than_Zero()
        {
            var mainVm = new TimelineViewModel();

            var componentVm = new TimelineComponentViewModel() { Start = 0, End = 100 };
            mainVm.Components.Add(componentVm);

            componentVm.UpdateStartCommand.Execute(-1.0);

            Assert.AreEqual(0, componentVm.Start);
        }
    }
}