namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals
{
    using System;
    using System.Collections.Generic;

    public class ProcessNextSegmentSignal
    {
        public List<Tuple<string, int>> SegmentIds { get; set; }

        public ProcessNextSegmentSignal() { }

        public ProcessNextSegmentSignal(List<Tuple<string, int>> segmentIds) { this.SegmentIds = segmentIds; }
    }
}