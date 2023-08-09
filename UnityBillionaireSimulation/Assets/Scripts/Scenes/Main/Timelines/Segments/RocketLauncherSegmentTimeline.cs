namespace TheOneStudio.HyperCasual.Scenes.Main.Timelines.Segments
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using UnityEngine;

    public class RocketLauncherSegmentTimeline: SegmentTimeline
    {
        public void ChangeSkybox()
        {
            Debug.Log("Fuckkkkkkkkkkkkkkkk ChangeSkybox");
            // var skybox = this.GetCurrentContainer().Resolve<ISkyboxService>();
            // skybox.ChangeSkybox();
        }
        
        public void AddStars()
        {
            Debug.Log("Fuckkkkkkkkkkkkkkkk AddStar");
            // var skybox = this.GetCurrentContainer().Resolve<ISkyboxService>();
            // skybox.AddStars();
        }
    }
}