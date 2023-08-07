namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using UnityEngine;
    using Zenject;

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public class PlayerInputSystem : ITickable
    {
        private const bool  IsUseTouchPhase     = true;
        private const float MinDistanceForSwipe = 200f;

        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;

        private readonly SignalBus signalBus;

        public PlayerInputSystem(SignalBus signalBus) { this.signalBus = signalBus; }

        public void Tick()
        {
            if (IsUseTouchPhase)
            {
                this.TouchByTouchPhase();
            }
            else
            {
                this.TouchByMouse();
            }
        }

        private void TouchByMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.OnTouchDown(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                this.OnDrag(Input.mousePosition, Input.mouseScrollDelta);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.OnTouchUp(Input.mousePosition);
            }
        }

        private void TouchByTouchPhase()
        {
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    this.OnTouchDown(touch.position);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    this.OnDrag(touch.position, touch.deltaPosition);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                default:
                    this.OnTouchUp(touch.position);
                    break;
            }
        }

        private void OnTouchDown(Vector3 pos)
        {
            this.fingerDownPosition = pos;
            this.signalBus.Fire(new InputTouchDownSignal(pos));
        }

        private void OnDrag(Vector2 mousePosition, Vector2 mouseScrollDelta) { this.signalBus.Fire(new InputDragSignal(mousePosition, mouseScrollDelta)); }

        private void OnTouchUp(Vector2 mousePosition)
        {
            this.fingerUpPosition = mousePosition;
            this.signalBus.Fire(new InputTouchUpSignal(mousePosition));
            this.CheckSwipe();
        }

        private void CheckSwipe()
        {
            var deltaX = this.fingerUpPosition.x - this.fingerDownPosition.x;
            var deltaY = this.fingerUpPosition.y - this.fingerDownPosition.y;

            if (!(Mathf.Abs(deltaX) > MinDistanceForSwipe) && !(Mathf.Abs(deltaY) > MinDistanceForSwipe)) return;
            this.signalBus.Fire(Mathf.Abs(deltaX) > Mathf.Abs(deltaY)
                ? new InputSwipeSignal(deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left)
                : new InputSwipeSignal(deltaY > 0 ? SwipeDirection.Up : SwipeDirection.Down));
        }
    }
}