namespace TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.States.CreateCharacter
{
    using GameFoundation.Scripts.Utilities;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.CreateCharacter;
    using Zenject;

    public class GiveCharacterNameState : BaseGameState
    {
        private readonly UserLocalData      userLocalData;
        public override  GameStateType      GameStateType { get; set; } = GameStateType.GiveCharacterName;
        public override  string             SegmentId     { get; set; }
        private readonly IAudioService      audioService;
        private readonly MiscParamBlueprint miscParamBlueprint;

        public GiveCharacterNameState(ScreenHandler screenHandler, SignalBus signalBus, UserLocalData userLocalData, IAudioService audioService, MiscParamBlueprint miscParamBlueprint) : base(
            screenHandler, signalBus)
        {
            this.userLocalData      = userLocalData;
            this.audioService       = audioService;
            this.miscParamBlueprint = miscParamBlueprint;
        }

        public override async void Enter()
        {
            this.audioService.StopAllPlayList();
            this.audioService.PlayPlayList(this.miscParamBlueprint.HomeMusic);
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