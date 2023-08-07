namespace TheOneStudio.HyperCasual.Models
{
    using GameFoundation.Scripts.Interfaces;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;

    public class UserLocalData : ILocalData
    {
        public void Init() { }

        public string          CharacterName      { get; set; }
        public long            CurrentScore    { get; set; }
    }
}