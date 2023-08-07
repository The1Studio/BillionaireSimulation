namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates;
    using Zenject;

    public class MergeMoneyState : BaseGameState
    {
        public override GameStateType GameStateType { get; set; } = GameStateType.MergeMoney;
        public override string        SegmentId     { get; set; }

        public MergeMoneyState(ScreenHandler screenHandler, SignalBus signalBus) : base(screenHandler, signalBus) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}