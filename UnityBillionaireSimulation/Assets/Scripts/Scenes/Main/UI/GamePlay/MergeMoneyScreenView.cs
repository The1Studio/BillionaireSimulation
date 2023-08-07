namespace TheOneStudio.HyperCasual.Scenes.Main.UI.GamePlay
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Zenject;

    public class MergeMoneyScreenView : BaseView
    {

    }

    [ScreenInfo(nameof(MergeMoneyScreenView))]
    public class MergeMoneyScreenPresenter : BaseScreenPresenter<MergeMoneyScreenView>
    {
        public MergeMoneyScreenPresenter(SignalBus signalBus) : base(signalBus)
        {
        }
        public override UniTask BindData()
        {
            return UniTask.CompletedTask;
        }
    }


}