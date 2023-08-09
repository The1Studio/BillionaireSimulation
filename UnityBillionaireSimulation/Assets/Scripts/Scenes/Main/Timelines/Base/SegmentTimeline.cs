namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using UnityEngine;
    using UnityEngine.Playables;
    using Zenject;

    public class SegmentTimeline : MonoBehaviour
    {
        private PlayableDirector director;
        public  PlayableDirector Director => this.director ??= this.GetComponent<PlayableDirector>();

        private SignalBus signalBus;
        private SignalBus SignalBus => this.signalBus ??= this.GetCurrentContainer().Resolve<SignalBus>();

        [Inject]
        private async void Constructor(DiContainer diContainer, SignalBus signal) { this.signalBus = signal; }

        private void OnEnable()  { this.OnEnter(); }
        private void OnDisable() { this.OnExit(); }

        protected virtual void OnEnter() { this.Director.stopped += this.OnPlayableDirectorCompleted; }
        
        protected virtual void OnExit() { this.Director.stopped -= this.OnPlayableDirectorCompleted; }

        private void OnPlayableDirectorCompleted(PlayableDirector obj) { this.SignalBus.Fire(new FinishCutSceneSignal()); }
    }
}