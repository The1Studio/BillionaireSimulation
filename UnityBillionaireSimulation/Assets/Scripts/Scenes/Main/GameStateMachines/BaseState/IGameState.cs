namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState
{
    using TheOneStudio.HyperCasual.Others.StateMachine.Interface;

    public interface IGameState : IState
    {
        GameStateType GameStateType { get; set; }
        string        SegmentId     { get; set; }
    }
}