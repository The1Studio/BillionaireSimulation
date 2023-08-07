namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals
{
    using TheOneStudio.HyperCasual.Blueprints;

    public class PickActionSignal
    {
        public string                 SegmentId    { get; set; }
        public PickActionChoiceRecord ChoiceRecord { get; set; }

        public PickActionSignal(string segmentId, PickActionChoiceRecord choiceRecord)
        {
            this.SegmentId    = segmentId;
            this.ChoiceRecord = choiceRecord;
        }
    }
}