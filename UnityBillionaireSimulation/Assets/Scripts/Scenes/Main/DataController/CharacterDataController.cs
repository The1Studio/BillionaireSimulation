namespace TheOneStudio.HyperCasual.Scenes.Main.DataController
{
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;

    public class CharacterDataController : BaseInventoryDataController
    {
        public override string CategoryName { get; set; } = "Character";
        public CharacterDataController(UITemplateInventoryDataController inventoryDataController, UserLocalData userLocalData) : base(inventoryDataController, userLocalData) { }
    }
}