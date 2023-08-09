using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates;
using UnityEngine;
using Zenject;

namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    using GameFoundation.Scripts.Utilities;
    using TheOneStudio.HyperCasual.Blueprints;

    public class LeaderboardState: BaseGameState
    {
        private readonly DiContainer   container;
        public override  GameStateType GameStateType { get; set; } = GameStateType.Leaderboard;
        public override  string        SegmentId     { get; set; }
        
        private readonly IAudioService      audioService;
        private readonly MiscParamBlueprint miscParamBlueprint;

        public LeaderboardState(ScreenHandler screenHandler, SignalBus signalBus, DiContainer container, IAudioService audioService, MiscParamBlueprint miscParamBlueprint) : base(screenHandler,
            signalBus)
        {
            this.container          = container;
            this.miscParamBlueprint = miscParamBlueprint;
            this.audioService       = audioService;
        }

        public override async void Enter()
        {
            this.audioService.StopAllPlayList();
            this.audioService.PlaySound(this.miscParamBlueprint.MusicWinAndRanking);
            await this.ScreenHandler.OpenPopup<LeaderBoardScreenPresenter, LeaderboardScreenModel>(new LeaderboardScreenModel()
            {
                OnOkClicked = this.OnLeaderboardOkClicked
            });
        }

        private void OnLeaderboardOkClicked() { this.NextState(); }

        public override void Exit()
        {
            base.Exit();
        }
    }
}