namespace TheOneStudio.HyperCasual.Scenes.Main.Timelines.Segments
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using UnityEngine;

    public class RocketLauncherSegmentTimeline : SegmentTimeline
    {
        private Color skyColor;
        private Color equatorColor;
        private float skyboxOffset;

        public Color newSkyColor;
        public Color newEquatorColor;
        public float skyboxNewOffset;

        private const float ChangeSkyboxDuration = 6f;

        private static readonly int SkyColor     = Shader.PropertyToID("_SkyColor");
        private static readonly int EnableStars  = Shader.PropertyToID("_EnableStars");
        private static readonly int EquatorColor = Shader.PropertyToID("_EquatorColor");
        private static readonly int SkyboxOffset = Shader.PropertyToID("_SkyboxOffset");

        private List<Tween> tweenSkies = new();
        private Material      skyboxMat;

        // Call by timeline signal
        public void ChangeSkybox()
        {
            this.tweenSkies.Add(DOTween.To(() => this.skyboxMat.GetColor(SkyColor), x => this.skyboxMat.SetColor(SkyColor, x), this.newSkyColor, ChangeSkyboxDuration));
            this.tweenSkies.Add(DOTween.To(() => this.skyboxMat.GetColor(EquatorColor), x => this.skyboxMat.SetColor(EquatorColor, x), this.newEquatorColor, ChangeSkyboxDuration));
            this.tweenSkies.Add(DOTween.To(() => this.skyboxMat.GetFloat(SkyboxOffset), x => this.skyboxMat.SetFloat(SkyboxOffset, x), this.skyboxNewOffset, ChangeSkyboxDuration));
        }

        // Call by timeline signal
        public void AddStars()
        {
            this.skyboxMat.SetFloat(EnableStars, 1);
        }

        private void ChangeToOriginSkybox()
        {
            this.skyboxMat.SetColor(SkyColor, this.skyColor);
            this.skyboxMat.SetColor(EquatorColor, this.equatorColor);
            this.skyboxMat.SetFloat(SkyboxOffset, this.skyboxOffset);
        }

        private void RemoveStars()
        {
            this.skyboxMat.SetFloat(EnableStars, 0);
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            this.skyboxMat = GetSkyBox();
            
            //Store origin skybox
            this.skyColor     = this.skyboxMat.GetColor(SkyColor);
            this.equatorColor = this.skyboxMat.GetColor(EquatorColor);
            this.skyboxOffset = this.skyboxMat.GetFloat(SkyboxOffset);
        }

        protected override void OnExit()
        {
            base.OnExit();
            this.ChangeToOriginSkybox();
            this.RemoveStars();

            foreach (var tweenSky in this.tweenSkies.Where(tweenSky => !tweenSky.IsActive()))
            {
                tweenSky.Kill();
            }
        }

        private static Material GetSkyBox() { return RenderSettings.skybox; }
    }
}