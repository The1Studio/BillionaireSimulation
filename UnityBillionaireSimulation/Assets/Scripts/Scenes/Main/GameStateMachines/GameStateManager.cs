namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.LogService;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Others.StateMachine.Interface;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Controller;
    using UnityEngine;
    using Zenject;

    public enum GameStateType
    {
        GiveCharacterName,
        CutScene,
        PickAction,
        FollowerShowOff,
        DelayInterval,
        MergeMoney
    }

    public class GameStateManager : StateMachine, IInitializable, IDisposable
    {
        private Dictionary<GameStateType, IGameState> TypeToGameState          { get; } = new();
        private int                                   CurrentStateIndex        { get; set; }
        private List<Tuple<string, int>>              SegmentIdToTimelineIndex { get; set; } = new();


        #region Inject

        private readonly GameplayMapLevelBlueprint     gameplayMapLevelBlueprint;
        private readonly UITemplateLevelDataController levelDataController;
        private readonly LevelHelper                   levelHelper;
        private readonly MapLevelSystem                mapLevelSystem;
        private readonly SegmentBlueprintHelper        segmentBlueprintHelper;
        private readonly UserLocalData                 userLocalData;

        public GameStateManager(List<IState> listState, SignalBus signalBus, ILogService logService, GameplayMapLevelBlueprint gameplayMapLevelBlueprint,
            UITemplateLevelDataController levelDataController, LevelHelper levelHelper, MapLevelSystem mapLevelSystem, SegmentBlueprintHelper segmentBlueprintHelper, UserLocalData userLocalData)
            : base(listState, signalBus, logService)
        {
            this.gameplayMapLevelBlueprint = gameplayMapLevelBlueprint;
            this.levelDataController       = levelDataController;
            this.levelHelper               = levelHelper;
            this.mapLevelSystem            = mapLevelSystem;
            this.segmentBlueprintHelper    = segmentBlueprintHelper;
            this.userLocalData             = userLocalData;

            foreach (var gameState in listState.Cast<IGameState>())
            {
                this.TypeToGameState.Add(gameState.GameStateType, gameState);
            }
        }

        #endregion

        public void Initialize()
        {
            this.signalBus.Subscribe<ChangeGameStateSignal>(this.OnStateChanged);
            this.signalBus.Subscribe<ProcessNextSegmentSignal>(this.OnProcessNextGameState);

            this.ResetCache();
        }

        private void ResetCache()
        {
            if (this.levelDataController.CurrentLevel != 1) return;

            this.userLocalData.CharacterName      = "";
            this.userLocalData.CurrentScore    = 0;
        }

        public void Dispose()
        {
            this.signalBus.Unsubscribe<ChangeGameStateSignal>(this.OnStateChanged);
            this.signalBus.Unsubscribe<ProcessNextSegmentSignal>(this.OnProcessNextGameState);
        }

        public void ProcessFirstSegment()
        {
            this.CurrentStateIndex = -1;
            this.SegmentIdToTimelineIndex.Clear();
            this.signalBus.Fire(new ProcessNextSegmentSignal());
        }

        private async void OnProcessNextGameState(ProcessNextSegmentSignal obj)
        {
            var currentMapLevelRecord = this.levelHelper.GetMapLevelRecordByLevel();
            if (currentMapLevelRecord == null)
            {
                this.logService.Log($"Can't find level {this.levelDataController.CurrentLevel} in GameplayMapLevelBlueprint", LogLevel.ERROR);
                return;
            }

            this.CurrentStateIndex++;
            if (this.CurrentStateIndex == 0)
            {
                this.SegmentIdToTimelineIndex = currentMapLevelRecord.SegmentIdToTimelineIndex;
                await this.mapLevelSystem.Generate(this.levelDataController.CurrentLevel);
            }
            else if (this.CurrentStateIndex >= this.SegmentIdToTimelineIndex.Count)
            {
                this.logService.Log($"Pass current level");
                this.levelDataController.PassCurrentLevel();

                this.ProcessFirstSegment();
                return;
            }
            else if (obj.SegmentIds != null)
            {
                this.SegmentIdToTimelineIndex.InsertRange(this.CurrentStateIndex, obj.SegmentIds);
            }

            var nextSegment = this.SegmentIdToTimelineIndex[this.CurrentStateIndex];
            this.ProcessSegment(nextSegment.Item1, nextSegment.Item2 - 1);
        }

        private void ProcessSegment(string nextSegmentId, int nextTimelineIndex)
        {
            var gameStateType = this.segmentBlueprintHelper.GetGameStateTypeBySegmentId(nextSegmentId);
            if (!this.TypeToGameState.TryGetValue(gameStateType, out var nextGameState))
            {
                this.logService.Log($"Can't find segment {nextSegmentId} in GameStateManager", LogLevel.ERROR);
                return;
            }

            nextGameState.SegmentId = nextSegmentId;

            this.signalBus.Fire(new ChangeGameStateSignal(nextGameState.GetType()) { NextSegmentId = nextSegmentId, NextTimelineIndex = nextTimelineIndex });
        }

        private void OnStateChanged(ChangeGameStateSignal obj)
        {
            this.TransitionTo(obj.StateType);
            this.logService.Log($"Enter {obj.StateType.Name} State!!!");
        }
    }
}