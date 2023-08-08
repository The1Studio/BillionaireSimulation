namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.MergeMoney;
    using Zenject;

    public class MergeMoneyState : BaseGameState
    {
        public override GameStateType GameStateType { get; set; } = GameStateType.MergeMoney;
        public override string        SegmentId     { get; set; }

        public MergeMoneyState(ScreenHandler screenHandler, SignalBus signalBus) : base(screenHandler, signalBus) { }

        public override async void Enter()
        {
            this.SignalBus.Subscribe<CompleteMergeGameSignal>(this.ChangeToNextState);
            this.ScreenHandler.CloseAllScreen();
            await this.ScreenHandler.OpenScreen<MergeMoneyScreenPresenter>();
            base.Enter();
        }

        private void ChangeToNextState()
        {
            this.NextState();
        }

        public override void Exit()
        {
            this.SignalBus.TryUnsubscribe<CompleteMergeGameSignal>(this.ChangeToNextState);
            base.Exit();
        }
    }
}