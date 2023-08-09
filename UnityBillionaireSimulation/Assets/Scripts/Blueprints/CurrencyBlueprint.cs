namespace TheOneStudio.HyperCasual.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [CsvHeaderKey("CurrencyId")]
    [BlueprintReader("CurrencyData", true)]
    public class CurrencyBlueprint : GenericBlueprintReaderByRow<string, CurrencyRecord>
    {
        
    }

    public class CurrencyRecord
    {
        public string CurrencyId;
        public int    Value;
        public string PrefabName;
        public string MergeUpTo;
        public string VfxMergeComplete;
    }
}