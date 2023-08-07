namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using TheOneStudio.HyperCasual.Others;

    public class CountFollowerSignal
    {
        public float Duration { get; set; }

        public CountFollowerSignal(float duration = GamePlayConstants.FollowerShowOffDuration) { this.Duration = duration; }
    }
}