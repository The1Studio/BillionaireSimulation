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
        public Dictionary<GameStateType, IGameState> TypeToGameState          { get; } = new();


        #region Inject

        private readonly GameplayMapLevelBlueprint     gameplayMapLevelBlueprint;
        private readonly UITemplateLevelDataController levelDataController;
        private readonly LevelHelper                   levelHelper;
        private readonly GenerateMapLevelSystem                generateMapLevelSystem;
        private readonly SegmentBlueprintHelper        segmentBlueprintHelper;
        private readonly UserLocalData                 userLocalData;

        public GameStateManager(List<IState> listState, SignalBus signalBus, ILogService logService, GameplayMapLevelBlueprint gameplayMapLevelBlueprint,
            UITemplateLevelDataController levelDataController, LevelHelper levelHelper, GenerateMapLevelSystem generateMapLevelSystem, SegmentBlueprintHelper segmentBlueprintHelper, UserLocalData userLocalData)
            : base(listState, signalBus, logService)
        {
            this.gameplayMapLevelBlueprint = gameplayMapLevelBlueprint;
            this.levelDataController       = levelDataController;
            this.levelHelper               = levelHelper;
            this.generateMapLevelSystem            = generateMapLevelSystem;
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
        }
        
        

        private void OnStateChanged(ChangeGameStateSignal obj)
        {
            this.TransitionTo(obj.StateType);
            this.logService.Log($"Enter {obj.StateType.Name} State!!!");
        }
    }
}