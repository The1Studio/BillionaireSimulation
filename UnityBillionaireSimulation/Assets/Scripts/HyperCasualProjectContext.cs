namespace TheOneStudio.HyperCasual
{
    using GameFoundation.Scripts;
    using TheOneStudio.UITemplate.UITemplate.Installers;
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
        }
    }
}