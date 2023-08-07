namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using TheOneStudio.HyperCasual.Others;

    public class ChangeStatusSignal
    {
        public float Duration { get; set; }

        public ChangeStatusSignal() { }

        public ChangeStatusSignal(float duration = GamePlayConstants.DialogueDuration) { this.Duration = duration; }
    }
}