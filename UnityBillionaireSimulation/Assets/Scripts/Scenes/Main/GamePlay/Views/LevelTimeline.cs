namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using UnityEngine;
    using Zenject;

    public class LevelTimeline : MonoBehaviour
    {
        private SignalBus              SignalBus              { get; set; }
        private GameStateManager       GameStateManager       { get; set; }
        private SegmentBlueprintHelper SegmentBlueprintHelper { get; set; }

        private List<SegmentTimeline> segmentTimelines;
        public  List<SegmentTimeline> SegmentTimelines => this.segmentTimelines ??= this.GetComponentsInChildren<SegmentTimeline>(true).ToList();

        [Inject]
        private void Constructor(SignalBus signalBus, GameStateManager gameStateManager, SegmentBlueprintHelper segmentBlueprintHelper)
        {
            this.SignalBus              = signalBus;
            this.GameStateManager       = gameStateManager;
            this.SegmentBlueprintHelper = segmentBlueprintHelper;

            this.SignalBus.Subscribe<ChangeGameStateSignal>(this.OnChangeGameState);
        }

        private void OnDestroy() { this.SignalBus.Unsubscribe<ChangeGameStateSignal>(this.OnChangeGameState); }

        private void OnChangeGameState(ChangeGameStateSignal obj)
        {
            var index = obj.NextTimelineIndex;
            if (index >= 0) this.ActiveSegment(index);
        }

        public void ActiveSegment(int index)
        {
            this.DeActiveAllSegments();
            this.SegmentTimelines[index].gameObject.SetActive(true);
            this.SegmentTimelines[index].Director.Play();
        }

        public void DeActiveAllSegments()
        {
            foreach (var segmentTimeline in this.SegmentTimelines)
            {
                segmentTimeline.gameObject.SetActive(false);
            }
        }

        public void Dispose() { Destroy(this.gameObject); }
    }
}