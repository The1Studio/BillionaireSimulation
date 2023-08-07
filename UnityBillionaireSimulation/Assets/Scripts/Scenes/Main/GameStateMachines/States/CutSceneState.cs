namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates;
    using Zenject;

    public class CutSceneState : BaseGameState
    {
        public override GameStateType GameStateType { get; set; } = GameStateType.CutScene;
        public override string        SegmentId     { get; set; }

        public CutSceneState(ScreenHandler screenHandler, SignalBus signalBus) : base(screenHandler, signalBus) { }

        public override void Enter()
        {
            base.Enter();
            this.SignalBus.Subscribe<FinishCutSceneSignal>(this.OnFinishCutScene);
        }

        public override void Exit()
        {
            base.Exit();
            this.SignalBus.Unsubscribe<FinishCutSceneSignal>(this.OnFinishCutScene);
        }

        private void OnFinishCutScene() { this.NextState(); }
    }
}