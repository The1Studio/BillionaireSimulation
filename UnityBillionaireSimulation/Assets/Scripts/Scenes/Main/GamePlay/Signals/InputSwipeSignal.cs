namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;

    public class InputSwipeSignal
    {
        public SwipeDirection SwipeDirection;
        public InputSwipeSignal(SwipeDirection swipeDirection) => this.SwipeDirection = swipeDirection;
    }
}