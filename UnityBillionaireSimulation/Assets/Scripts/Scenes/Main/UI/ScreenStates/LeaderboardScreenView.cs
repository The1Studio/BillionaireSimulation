using System;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.Utilities.LogService;
using TheOneStudio.UITemplate.UITemplate.Scenes.Utils;
using UnityEngine.UI;
using Zenject;

namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates
{
    public class LeaderboardScreenView: BaseView
    {
        public Button btnOk;
    }
    
    public class LeaderboardScreenModel
    {
        public Action OnOkClicked;
    }
    
    [ScreenInfo(nameof(LeaderboardScreenView))]
    public class LeaderboardScreenPresenter : UITemplateBaseScreenPresenter<LeaderboardScreenView, LeaderboardScreenModel>
    {
        public LeaderboardScreenPresenter(SignalBus signalBus, ILogService logService) : base(signalBus, logService) { }

        public override UniTask BindData(LeaderboardScreenModel popupModel) { return UniTask.CompletedTask; }

        protected override void OnViewReady()
        {
            base.OnViewReady();

            this.View.btnOk.onClick.AddListener(this.OnOkClicked);
        }

        private void OnOkClicked() { this.Model.OnOkClicked?.Invoke(); }
    }
    

}