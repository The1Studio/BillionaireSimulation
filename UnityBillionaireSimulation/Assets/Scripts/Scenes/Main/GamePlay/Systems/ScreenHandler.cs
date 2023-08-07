namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;

    public class ScreenHandler
    {
        #region Inject

        private readonly IScreenManager screenManager;

        #endregion

        public ScreenHandler(IScreenManager screenManager) { this.screenManager = screenManager; }

        public RootUICanvas RootUICanvas => this.screenManager.RootUICanvas;

        public async UniTask<TPresenter> OpenScreen<TPresenter>() where TPresenter : IScreenPresenter
        {
            await this.CloseAllScreen();
            var screen = await this.screenManager.OpenScreen<TPresenter>();
            return screen;
        }

        public async UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel> where TModel : class
        {
            await this.CloseAllScreen();
            var screen = await this.screenManager.OpenScreen<TPresenter, TModel>(model);
            return screen;
        }

        public async UniTask<TPresenter> OpenPopup<TPresenter>() where TPresenter : IScreenPresenter
        {
            await this.CloseAllScreen();
            var screen = await this.screenManager.OpenScreen<TPresenter>();
            return screen;
        }

        public async UniTask<TPresenter> OpenPopup<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel> where TModel : class
        {
            var screen = await this.screenManager.OpenScreen<TPresenter, TModel>(model);
            return screen;
        }

        public UniTask CloseAllScreen() { return this.screenManager.CloseAllScreenAsync(); }

        public IScreenPresenter CurrentActiveScreen => this.screenManager.CurrentActiveScreen.Value;
    }
}