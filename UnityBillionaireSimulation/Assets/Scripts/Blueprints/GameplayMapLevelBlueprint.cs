namespace TheOneStudio.HyperCasual.Blueprints
{
    using System;
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;
    using UnityEngine.Serialization;

    [CsvHeaderKey("Level")]
    [BlueprintReader("GameplayMapLevel", true)]
    public class GameplayMapLevelBlueprint : GenericBlueprintReaderByRow<int, GameplayMapLevelRecord>
    {
    }

    public class GameplayMapLevelRecord
    {
        public int                      Level;
        public string                   Name;
        public string                   PrefabAsset;
        public List<Tuple<string, int>> SegmentIdToTimelineIndex;
    }
}