namespace TheOneStudio.HyperCasual.Scenes.Main
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Main;
    using Zenject;

    [ScreenInfo("HomeSimpleScreenView")]
    public class HomeSimpleScreenPresenter : UITemplateHomeSimpleScreenPresenter
    {
        public HomeSimpleScreenPresenter(SignalBus signalBus, DiContainer diContainer, IScreenManager screenManager, UITemplateInventoryDataController uiTemplateInventoryDataController) : base(signalBus, diContainer, screenManager, uiTemplateInventoryDataController)
        {
        }
    }
}