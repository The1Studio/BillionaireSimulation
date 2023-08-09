namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ButtonTool : MonoBehaviour
    {
        [Inject] private readonly SignalBus signalBus;
        private                   Button    button;
        public void Start()
        {
            this.GetCurrentContainer().Inject(this);
            this.button = this.GetComponent<Button>();
            this.button.onClick.AddListener(this.OpenTool);
        }

        private void OpenTool()
        {
            this.signalBus.Fire(new OpenToolSignal());
        }
        
    }
}