namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using UnityEngine;

    public class InputTouchDownSignal
    {
        public Vector2 TouchPosition;
        public InputTouchDownSignal(Vector2 touchPosition) { this.TouchPosition = touchPosition; }
    }
}