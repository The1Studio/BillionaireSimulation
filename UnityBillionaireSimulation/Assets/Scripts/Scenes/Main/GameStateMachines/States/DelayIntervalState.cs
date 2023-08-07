namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    using Cysharp.Threading.Tasks;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Others;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using Zenject;

    public class DelayIntervalState : BaseGameState
    {
        private readonly SegmentDelayIntervalBlueprint delayIntervalBlueprint;
        public override  GameStateType                 GameStateType { get; set; } = GameStateType.DelayInterval;
        public override  string                        SegmentId     { get; set; }

        public DelayIntervalState(ScreenHandler screenHandler, SignalBus signalBus, SegmentDelayIntervalBlueprint delayIntervalBlueprint) : base(screenHandler, signalBus)
        {
            this.delayIntervalBlueprint = delayIntervalBlueprint;
        }

        public override async void Enter()
        {
            base.Enter();
            this.ScreenHandler.CloseAllScreen();
            await UniTask.Delay(this.delayIntervalBlueprint.GetDataById(this.SegmentId).Duration.ToMilliseconds());
            this.NextState();
        }
    }
}