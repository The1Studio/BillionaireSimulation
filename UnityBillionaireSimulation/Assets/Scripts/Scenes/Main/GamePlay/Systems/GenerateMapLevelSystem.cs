namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.LogService;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.DataController;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using TheOneStudio.UITemplate.UITemplate.Blueprints;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;

    public class GenerateMapLevelSystem : IInitializable

    {
        public MapData CurrentMapData { get; set; }

        #region inject

        private readonly ILogService                   logService;
        private readonly UserLocalData                 userLocalData;
        private readonly GameplayMapLevelBlueprint     mapLevelBlueprint;
        private readonly CharacterDataController       characterDataController;
        private readonly UITemplateItemBlueprint       uiTemplateItemBlueprint;
        private readonly UITemplateLevelDataController levelDataController;
        private readonly IGameAssets                   gameAssets;
        private readonly DiContainer                   container;

        #endregion

        public GenerateMapLevelSystem(ILogService logService, UserLocalData userLocalData, GameplayMapLevelBlueprint mapLevelBlueprint,
            CharacterDataController characterDataController, UITemplateItemBlueprint uiTemplateItemBlueprint, IGameAssets gameAssets, DiContainer container,UITemplateLevelDataController levelDataController)
        {
            this.logService              = logService;
            this.userLocalData           = userLocalData;
            this.mapLevelBlueprint       = mapLevelBlueprint;
            this.characterDataController = characterDataController;
            this.uiTemplateItemBlueprint = uiTemplateItemBlueprint;
            this.gameAssets              = gameAssets;
            this.container               = container;
            this.levelDataController     = levelDataController;
        }
        
        public void Initialize() { this.GenerateMapLevel(this.levelDataController.CurrentLevel); }

        private void GenerateMapLevel(int level)
        {
            this.logService.Log("Generate Map " + level);
            this.DestroyOldMapData();
            this.SpawnSingleLevel(level);
        }

        private async void SpawnSingleLevel(int level)
        {
            var gameplayMapLevelRecord = this.mapLevelBlueprint.GetDataById(level);
            var prefab                 = await this.gameAssets.LoadAssetAsync<GameObject>(gameplayMapLevelRecord.PrefabAsset);
            var levelObject            = Object.Instantiate(prefab);
            var levelController        = levelObject.GetComponent<LevelController>();
            this.container.InjectGameObject(levelObject);
            var cam = levelObject.GetComponentInChildren<Camera>();
            this.CurrentMapData = new MapData()
            {
                LevelController = levelController,
                Camera          = cam
            };

            levelObject.GetComponent<LevelController>().ListSegmentInLevel = gameplayMapLevelRecord.ListSegment.Select(e => new Tuple<string, bool>(e, false)).ToList();
        }

        private void DestroyOldMapData() { this.CurrentMapData?.Dispose(); }

        public class MapData : IDisposable
        {
            public LevelController LevelController { get; set; }
            public Camera          Camera          { get; set; }

            public void Dispose()
            {
                if (this.LevelController != null)
                {
                    this.LevelController.Dispose();
                }
            }
        }

        
    }
}