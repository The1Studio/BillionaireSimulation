namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.LogService;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.DataController;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using TheOneStudio.UITemplate.UITemplate.Blueprints;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;

    public class MapLevelSystem
    {
        public MapData CurrentMapData { get; set; }

        #region inject

        private readonly ILogService               logService;
        private readonly UserLocalData             userLocalData;
        private readonly GameplayMapLevelBlueprint mapLevelBlueprint;
        private readonly CharacterDataController   characterDataController;
        private readonly UITemplateItemBlueprint   uiTemplateItemBlueprint;
        private readonly IGameAssets               gameAssets;
        private readonly DiContainer               container;

        #endregion

        public MapLevelSystem(ILogService logService, UserLocalData userLocalData, GameplayMapLevelBlueprint mapLevelBlueprint,
            CharacterDataController characterDataController, UITemplateItemBlueprint uiTemplateItemBlueprint, IGameAssets gameAssets, DiContainer container)
        {
            this.logService              = logService;
            this.userLocalData           = userLocalData;
            this.mapLevelBlueprint       = mapLevelBlueprint;
            this.characterDataController = characterDataController;
            this.uiTemplateItemBlueprint = uiTemplateItemBlueprint;
            this.gameAssets              = gameAssets;
            this.container               = container;
        }

        public async UniTask Generate(int level)
        {
            this.logService.Log("Generate Map " + level);
            this.DestroyOldMapData();

            var gameplayMapLevelRecord = this.mapLevelBlueprint.GetDataById(level);

            var prefab        = await this.gameAssets.LoadAssetAsync<GameObject>(gameplayMapLevelRecord.PrefabAsset);
            var gameObject    = Object.Instantiate(prefab);
            var levelTimeline = gameObject.GetComponent<LevelTimeline>();
            this.container.InjectGameObject(gameObject);

            var cam = gameObject.GetComponentInChildren<Camera>();

            this.CurrentMapData = new MapData()
            {
                LevelTimeline = levelTimeline,
                Camera        = cam
            };
        }

        private void DestroyOldMapData() { this.CurrentMapData?.Dispose(); }

        public class MapData : IDisposable
        {
            public LevelTimeline LevelTimeline { get; set; }
            public Camera        Camera        { get; set; }

            public void Dispose()
            {
                if (this.LevelTimeline != null)
                {
                    this.LevelTimeline.Dispose();
                }
            }
        }
    }
}