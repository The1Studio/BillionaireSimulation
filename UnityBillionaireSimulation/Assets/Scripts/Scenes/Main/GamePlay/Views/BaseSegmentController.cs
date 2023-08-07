namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using System.Runtime.CompilerServices;
    using GameFoundation.Scripts.Utilities.Extension;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using UnityEngine;
    using UnityEngine.Playables;
    using Zenject;

    public abstract class BaseSegmentController : MonoBehaviour
    {
        #region Inject

        [Inject] public SignalBus signalBus;

        #endregion

        public             string        segmentId;
        public             bool          isCompleteSegment = false;
        protected abstract GameStateType GameStateForSegment { get; }

        private PlayableDirector director;
        private PlayableDirector Director => this.director ??= this.GetComponent<PlayableDirector>();

        public virtual void StartActiveSegment() { }

        protected virtual void Start()
        {
            this.GetCurrentContainer().Inject(this);
        }

        protected virtual void OnEnable() { this.Director.stopped += this.OnPlayableDirectorCompleted; }

        protected virtual void OnDisable() { this.Director.stopped -= this.OnPlayableDirectorCompleted; }

        protected virtual void OnPlayableDirectorCompleted(PlayableDirector obj) { this.signalBus.Fire(new FinishCutSceneSignal()); }
    }
}