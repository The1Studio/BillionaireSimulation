namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using UnityEngine;

    public class InputTouchUpSignal
    {
        public Vector2 Position;
        public InputTouchUpSignal(Vector2 position) { this.Position = position; }
    }
}