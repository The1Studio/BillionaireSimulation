namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.PickActions;
    using TheOneStudio.UITemplate.UITemplate.Scripts.ThirdPartyServices;
    using UnityEngine;
    using Zenject;

    public class PickActionState : BaseGameState
    {
        public override GameStateType GameStateType { get; set; } = GameStateType.PickAction;
        public override string        SegmentId     { get; set; }

        private readonly UserLocalData              userLocalData;
        private readonly UITemplateAdServiceWrapper adServiceWrapper;

        public PickActionState(ScreenHandler screenHandler, SignalBus signalBus, UserLocalData userLocalData, UITemplateAdServiceWrapper adServiceWrapper) : base(screenHandler, signalBus)
        {
            this.userLocalData    = userLocalData;
            this.adServiceWrapper = adServiceWrapper;
        }

        public override async void Enter()
        {
            await this.ScreenHandler.OpenScreen<PickActionScreenPresenter, PickActionScreenModel>(new PickActionScreenModel()
            {
                Id            = this.SegmentId,
                OnClickOption = this.OnClickAction
            });
        }

        private void OnClickAction(PickActionChoiceRecord obj)
        {
            if (obj.UnlockByAds)
            {
                this.adServiceWrapper.ShowRewardedAd("pick_action", () => this.OnProcessAction(obj));
            }
            else
            {
                this.OnProcessAction(obj);
            }
        }

        private void OnProcessAction(PickActionChoiceRecord obj)
        {
            this.SignalBus.Fire(new PickActionSignal(this.SegmentId, obj));
            this.userLocalData.CurrentScore += obj.Score;
            this.NextState(obj.NextSegmentIdToTimelineIndex);
        }
    }
}