namespace TheOneStudio.HyperCasual.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("MicsParam", true)]
    public class MiscParamBlueprint : GenericBlueprintReaderByCol
    {
        public int    TotalSlotMoneyInGame { get; set; }
        public string MusicInGame          { get; set; }
        public string MusicWinAndRanking   { get; set; }
        public string MergeSound           { get; set; }
        public string HumIdle              { get; set; }
        public string HomeMusic            { get; set; }
        public string CompleteMergeSound   { get; set; }

        public string VfxCompleteMerge { get; set; }

        public string FuelSound { get; set; }
    }
}