using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates;
using UnityEngine;
using Zenject;

namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States
{
    public class LeaderboardState: BaseGameState
    {
        private readonly DiContainer   container;
        public override  GameStateType GameStateType { get; set; } = GameStateType.Leaderboard;
        public override  string        SegmentId     { get; set; }

        public LeaderboardState(ScreenHandler screenHandler, SignalBus signalBus, DiContainer container) : base(screenHandler, signalBus) { this.container = container; }

        public override async void Enter()
        {
            await this.ScreenHandler.OpenScreen<LeaderboardScreenPresenter, LeaderboardScreenData>(new LeaderboardScreenData()
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