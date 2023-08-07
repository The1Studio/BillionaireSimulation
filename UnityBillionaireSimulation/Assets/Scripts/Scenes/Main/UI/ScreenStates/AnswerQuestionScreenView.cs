namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Utils;
    using Zenject;

    public class AnswerQuestionScreenView : BaseView
    {
    }

    [ScreenInfo(nameof(AnswerQuestionScreenView))]
    public class AnswerQuestionScreenPresenter : UITemplateBaseScreenPresenter<AnswerQuestionScreenView>
    {
        public AnswerQuestionScreenPresenter(SignalBus signalBus) : base(signalBus) { }

        public override UniTask BindData() { return UniTask.CompletedTask; }
    }
}