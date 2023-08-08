namespace TheOneStudio.HyperCasual.Scenes.Main.UI.Extension
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public static class UIEffectExtension
    {
        public static Tween DoFillAmount(this Image image, float newValue, float duration = 0.5f)
        {
            return DOTween.To(
                getter: () => image.fillAmount    = image.fillAmount,
                setter: value => image.fillAmount = value,
                endValue: newValue,
                duration: duration
            ).SetEase(Ease.Linear);
        }

        public static void DoSliderValue(this Slider slider, float newValue, float duration = 0.5f)
        {
            DOTween.To(
                getter: () => slider.value    = slider.value,
                setter: value => slider.value = value,
                endValue: newValue,
                duration: duration
            ).SetEase(Ease.Linear);
        }

        public static Tween DoAlpha(this Image image, float targetValue, float duration = 0.5f)
        {
            return DOTween.To(
                getter: () => image.color.a,
                setter: value => image.color = new Color(image.color.r, image.color.g, image.color.b, value),
                endValue: targetValue,
                duration: duration
            ).SetEase(Ease.Linear);
        }
    }
}