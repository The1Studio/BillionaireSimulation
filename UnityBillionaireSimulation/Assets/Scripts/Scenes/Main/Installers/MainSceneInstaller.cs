namespace TheOneStudio.HyperCasual.Scenes.Main.Installers
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Others.StateMachine.Interface;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.BaseState;
    using TheOneStudio.HyperCasual.Scenes.Main.GameStateMachines.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.GamePlay;
    using Zenject;

    public class MainSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.Container.InitScreenManually<GameplayScreenPresenter>();

            this.BindGameState();

            this.BindOther();
            this.DeclareSignals();
        }

        public void BindGameState()
        {
            this.Container.DeclareSignal<ChangeGameStateSignal>();
            this.Container.DeclareSignal<PickActionSignal>();
            this.Container.DeclareSignal<ProcessNextSegmentSignal>();

            this.Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();

            this.Container.Bind<IState>().To(x => x.AllTypes().Where(type => !type.IsAbstract).DerivingFrom<IGameState>()).WhenInjectedInto<GameStateManager>();
        }

        private void DeclareSignals()
        {
            this.Container.DeclareSignal<SpawnVfxSignal>();
            this.Container.DeclareSignal<InputDragSignal>();
            this.Container.DeclareSignal<InputTouchDownSignal>();
            this.Container.DeclareSignal<InputTouchUpSignal>();
            this.Container.DeclareSignal<InputSwipeSignal>();
            this.Container.DeclareSignal<ResetGameplaySignal>();
            this.Container.DeclareSignal<ChangeCharacterGenderSignal>();
            this.Container.DeclareSignal<ChangeAvatarSignal>();
            this.Container.DeclareSignal<CountFollowerSignal>();
            this.Container.DeclareSignal<ChangeStatusSignal>();
            this.Container.DeclareSignal<FinishCutSceneSignal>();
            this.Container.DeclareSignal<ChangeToNextSegmentSignal>();
        }

        private void BindOther()
        {
            this.Container.Bind<LevelResult>().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<GenerateMapLevelSystem>().AsCached().NonLazy();
            this.Container.Bind<SegmentBlueprintHelper>().AsCached();
        }
    }
}