namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Segment
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using UnityEngine;
    using Zenject;

    public class MergeMoneySegment : BaseSegmentController
    {
        #region Inject

        [Inject]
        private readonly GameStateManager gameStateManager;
        

        #endregion
        protected override GameStateType GameStateForSegment => GameStateType.MergeMoney;

        protected override void Start()
        {
            base.Start();
            var a= this.gameStateManager.TypeToGameState[this.GameStateForSegment].GetType();
            this.signalBus.Fire(new ChangeGameStateSignal((this.gameStateManager.TypeToGameState[this.GameStateForSegment]).GetType()));
        }
    }
}