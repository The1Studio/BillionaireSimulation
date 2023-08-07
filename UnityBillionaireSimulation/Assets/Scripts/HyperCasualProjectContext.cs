namespace TheOneStudio.HyperCasual
{
    using GameFoundation.Scripts;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.DataController;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Systems;
    using TheOneStudio.UITemplate.UITemplate.Installers;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Main.CollectionNew;
    using TheOneStudio.UITemplate.UITemplate.Services.Toast;
    using UnityEngine.EventSystems;
    using Zenject;

    public class HyperCasualProjectContext : MonoInstaller<HyperCasualProjectContext>
    {
        public ToastController ToastController;

        public override void InstallBindings()
        {
            //GDK stuff
            GameFoundationInstaller.Install(this.Container);

            //Global UI event system
            this.Container.Bind<EventSystem>().FromComponentInNewPrefabResource("EventSystem").AsCached().NonLazy();
            //UI template stuff
            UITemplateInstaller.Install(this.Container, this.ToastController);

            this.BindOthers();
        }

        private void BindOthers()
        {
            this.Container.Bind<LevelHelper>().AsCached().NonLazy();
            this.Container.Bind<CharacterDataController>().AsCached().NonLazy();
            this.Container.Bind<ScreenHandler>().AsCached().NonLazy();
        }
    }
}