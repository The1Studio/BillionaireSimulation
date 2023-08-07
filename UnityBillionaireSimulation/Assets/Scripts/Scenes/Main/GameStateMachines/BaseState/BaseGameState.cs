namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState
{
    using System;
    using System.Collections.Generic;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using Zenject;

    public abstract class BaseGameState : IGameState
    {
        protected readonly ScreenHandler ScreenHandler;
        protected readonly SignalBus     SignalBus;

        public BaseGameState(ScreenHandler screenHandler, SignalBus signalBus)
        {
            this.ScreenHandler = screenHandler;
            this.SignalBus     = signalBus;
        }

        public virtual void Enter() { }
        public virtual void Exit()  { }

        public abstract GameStateType GameStateType { get; set; }
        public abstract string        SegmentId     { get; set; }

        protected void NextState(List<Tuple<string, int>> NextSegmentIds = null) { this.SignalBus.Fire(new ProcessNextSegmentSignal(NextSegmentIds)); }
    }
}