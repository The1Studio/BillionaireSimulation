namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals
{
    using System;

    public class ChangeGameStateSignal
    {
        public Type   StateType         { get; }
        public string NextSegmentId     { get; set; }
        public int    NextTimelineIndex { get; set; }

        public ChangeGameStateSignal(Type stateType) { this.StateType = stateType; }
    }
}