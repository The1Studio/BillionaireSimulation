namespace TheOneStudio.HyperCasual.Scenes.Loading
{
    using BlueprintFlow.BlueprintControlFlow;
    using Core.AdsServices;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Loading;
    using TheOneStudio.UITemplate.UITemplate.Scripts.ThirdPartyServices;
    using TheOneStudio.UITemplate.UITemplate.UserData;
    using Zenject;

    [ScreenInfo("LoadingScreenView")]
    public class LoadingScenePresenter : UITemplateLoadingScreenPresenter
    {
        private const int MinTimeToLoading = 3000;
        protected LoadingScenePresenter(SignalBus signalBus, UITemplateAdServiceWrapper adService, IAOAAdService aoaAdService, BlueprintReaderManager blueprintManager, UserDataManager userDataManager, IGameAssets gameAssets, ObjectPoolManager objectPoolManager) : base(signalBus, adService, aoaAdService, blueprintManager, userDataManager, gameAssets, objectPoolManager)
        {
        }

        protected override async UniTask Preload()
        {
            await base.Preload();
            await UniTask.Delay(MinTimeToLoading);
        }
    }
}