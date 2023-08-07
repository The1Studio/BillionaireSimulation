namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;

    public class ChangeCharacterGenderSignal
    {
        public CharacterGender CharacterGender { get; set; }

        public ChangeCharacterGenderSignal(CharacterGender characterGender) { this.CharacterGender = characterGender; }
    }
}