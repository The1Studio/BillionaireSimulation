namespace TheOneStudio.HyperCasual.Scenes.Main.UI.GamePlay
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Utils;
    using Zenject;

    public class GameplayScreenView : BaseView
    {
        public HeaderScreenView headerScreenView;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : UITemplateBaseScreenPresenter<GameplayScreenView>
    {
        private readonly DiContainer           container;
        private readonly GameStateManager      gameStateManager;
        private          HeaderScreenPresenter headerScreenPresenter;

        public GameplayScreenPresenter(SignalBus signalBus, DiContainer container, GameStateManager gameStateManager) : base(signalBus)
        {
            this.container        = container;
            this.gameStateManager = gameStateManager;
        }

        public override UniTask BindData() { return UniTask.CompletedTask; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.InitHeaderScreenView();
        }

        private void InitHeaderScreenView()
        {
            this.headerScreenPresenter = this.container.Instantiate<HeaderScreenPresenter>();
            this.headerScreenPresenter.SetView(this.View.headerScreenView);
            this.headerScreenPresenter.BindData(new HeaderScreenModel());
        }
        
    }
}