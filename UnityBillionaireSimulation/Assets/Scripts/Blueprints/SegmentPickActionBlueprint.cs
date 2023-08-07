namespace TheOneStudio.HyperCasual.Blueprints
{
    using System;
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;

    public enum PickActionType
    {
        Horizontal,
        Vertical
    }

    [CsvHeaderKey("Id")]
    [BlueprintReader("SegmentPickAction", true)]
    public class SegmentPickActionBlueprint : GenericBlueprintReaderByRow<string, SegmentPickActionRecord>
    {
    }

    public class SegmentPickActionRecord
    {
        public string                                 Id;
        public GameStateType                          GameStateType;
        public PickActionType                         PickActionType;
        public BlueprintByRow<PickActionChoiceRecord> ChoiceRecords;
    }

    [CsvHeaderKey("ChoiceId")]
    public class PickActionChoiceRecord
    {
        public string                   ChoiceId;
        public bool                     UnlockByAds;
        public int                      Score;
        public string                   ImageAsset;
        public string                   Content;
        public List<Tuple<string, int>> NextSegmentIdToTimelineIndex;
    }
}