namespace TheOneStudio.HyperCasual.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;

    [CsvHeaderKey("Id")]
    [BlueprintReader("SegmentMiniGame", true)]
    public class SegmentMiniGameBlueprint : GenericBlueprintReaderByRow<string, SegmentMiniGameRecord>
    {
    }

    public class SegmentMiniGameRecord
    {
        public string        Id;
        public GameStateType GameStateType;
    }
}