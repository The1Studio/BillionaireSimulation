namespace TheOneStudio.HyperCasual.Models
{
    using System.Linq;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;

    public class LevelHelper
    {
        private readonly UserLocalData                 userLocalData;
        private readonly UITemplateLevelDataController levelDataController;
        private readonly GameplayMapLevelBlueprint     gameplayMapLevelBlueprint;

        public LevelHelper(UserLocalData userLocalData, UITemplateLevelDataController levelDataController, GameplayMapLevelBlueprint gameplayMapLevelBlueprint)
        {
            this.userLocalData             = userLocalData;
            this.levelDataController       = levelDataController;
            this.gameplayMapLevelBlueprint = gameplayMapLevelBlueprint;
        }

        public GameplayMapLevelRecord GetMapLevelRecordByLevel(int level) { return this.gameplayMapLevelBlueprint.Values.FirstOrDefault(record => record.Level == level); }
        public GameplayMapLevelRecord GetMapLevelRecordByLevel()          { return this.GetMapLevelRecordByLevel(this.levelDataController.CurrentLevel); }
    }
}