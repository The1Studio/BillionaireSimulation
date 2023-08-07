namespace TheOneStudio.HyperCasual.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("MicsParam", true)]
    public class MiscParamBlueprint : GenericBlueprintReaderByCol
    {
        public int TotalSlotMoneyInGame { get; set; }
    }
}