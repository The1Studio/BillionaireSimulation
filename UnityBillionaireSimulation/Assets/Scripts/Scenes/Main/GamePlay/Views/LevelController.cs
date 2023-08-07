namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using UnityEngine;
    using Zenject;

    public class LevelController : MonoBehaviour
    {
        #region Inject

        private SignalBus              SignalBus              { get; set; }
        private GameStateManager       GameStateManager       { get; set; }
        private SegmentBlueprintHelper SegmentBlueprintHelper { get; set; }
        private ObjectPoolManager      ObjectPoolManager      { get; set; }

        #endregion


        public List<Tuple<string, bool>> ListSegmentInLevel = new();
        public BaseSegmentController     currentSegmentController;
        public GameObject                currentSegmentGameObject;

        [Inject]
        private void Constructor(SignalBus signalBus, GameStateManager gameStateManager, SegmentBlueprintHelper segmentBlueprintHelper, ObjectPoolManager objectPoolManager)
        {
            this.SignalBus              = signalBus;
            this.GameStateManager       = gameStateManager;
            this.SegmentBlueprintHelper = segmentBlueprintHelper;
            this.ObjectPoolManager      = objectPoolManager;
        }

        private void Start()
        {
            this.HandleNextSegment();
            this.SignalBus.Subscribe<ChangeToNextSegmentSignal>(this.HandleNextSegment);
        }

        private void HandleNextSegment()
        {
            if (this.ListSegmentInLevel.All(segment => segment.Item2))
            {
                Debug.Log("Complete level");
                return;
            }
            var nextSegment = this.ListSegmentInLevel.FirstOrDefault(segment => !segment.Item2);
            if (nextSegment != null) this.SpawnSegmentGameObject(nextSegment.Item1);
        }

        private async void SpawnSegmentGameObject(string segmentPrefabName)
        {
            if(this.currentSegmentGameObject!=null) Destroy(this.currentSegmentGameObject);
            this.currentSegmentGameObject = await this.ObjectPoolManager.Spawn(segmentPrefabName,this.transform);
            this.currentSegmentController = this.currentSegmentGameObject.GetComponent<BaseSegmentController>();
            this.currentSegmentController.StartActiveSegment();
        }

        private void OnDestroy() { }

        public void Dispose()
        {
            this.SignalBus.TryUnsubscribe<ChangeToNextSegmentSignal>(this.HandleNextSegment);
            Destroy(this.gameObject);
        }
    }
}