namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States.CreateCharacter
{
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.CreateCharacter;
    using Zenject;

    public class GiveCharacterNameState : BaseGameState
    {
        private readonly UserLocalData userLocalData;
        public override  GameStateType GameStateType { get; set; } = GameStateType.GiveCharacterName;
        public override  string        SegmentId     { get; set; }

        public GiveCharacterNameState(ScreenHandler screenHandler, SignalBus signalBus, UserLocalData userLocalData) : base(screenHandler, signalBus) { this.userLocalData = userLocalData; }

        public override async void Enter()
        {
            await this.ScreenHandler.OpenScreen<GiveCharacterNameScreenPresenter, GiveCharacterNameScreenData>(new GiveCharacterNameScreenData()
            {
                OnAcceptName = this.OnAcceptNameClicked
            });
        }

        private void OnAcceptNameClicked(string obj)
        {
            this.userLocalData.CharacterName = obj;
            this.NextState();
        }
    }
}