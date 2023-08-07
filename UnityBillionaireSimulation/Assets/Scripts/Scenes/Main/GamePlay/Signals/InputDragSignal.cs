namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using UnityEngine;

    public class InputDragSignal
    {
        public Vector2 TouchPosition;
        public Vector2 DeltaPosition;
        public InputDragSignal(Vector2 touchPosition, Vector2 deltaPosition)
        {
            this.TouchPosition = touchPosition;
            this.DeltaPosition = deltaPosition;
        }
    }
}