namespace TheOneStudio.HyperCasual.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;

    [CsvHeaderKey("Id")]
    [BlueprintReader("SegmentDelayInterval", true)]
    public class SegmentDelayIntervalBlueprint : GenericBlueprintReaderByRow<string, SegmentDelayIntervalRecord>
    {
    }

    public class SegmentDelayIntervalRecord
    {
        public string        Id;
        public float         Duration;
        public GameStateType GameStateType;
    }
}