namespace TheOneStudio.HyperCasual.Scenes.Main.Services
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using QFSW.QC;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using UnityEngine;
    using Zenject;

    public class DebugService : MonoBehaviour
    {
        [Inject] private ObjectPoolManager               objectPoolManager;
        [Inject] private SignalBus                       signalBus;
        [Inject] private UITemplateSettingDataController settingDataController;
        private          GameObject                      consoleGameObject;
        private          bool                            isOpenConsole = false;
        private void Start()
        {
            this.GetCurrentContainer().Inject(this);
#if DEBUG_MODE
            this.SpawnQuantumObject();
#endif
        }

        #region ToolHandle

        private int   touchCount;
        private float touchTime = 0.25f;
        private float counter;

        private void OpenCheatingTool()
        {
            this.OpenConsole();
            this.touchCount = 0;
            this.counter    = 0;
        }

        public void Update()
        {
#if DEBUG_MODE
            
            this.counter = this.counter > 0 ? this.counter - Time.deltaTime : 0;
            if (this.counter == 0) this.touchCount = 0;

            if (!Input.GetMouseButtonDown(0)) return;
            this.counter = this.touchTime;
            this.touchCount++;

            if (this.touchCount != 3) return;
            this.OpenCheatingTool();
#endif
        }

        #endregion


        #region QuantumConsole

        private void OpenConsole()
        {
            if (this.consoleGameObject == null) return;
            if (!this.isOpenConsole)
            {
                this.consoleGameObject.GetComponent<QuantumConsole>().Activate();
                this.isOpenConsole = true;
            }
            else
            {
                this.consoleGameObject.GetComponent<QuantumConsole>().Deactivate();
                this.isOpenConsole = false;
            }
        }
        private async void SpawnQuantumObject() { this.consoleGameObject = await this.objectPoolManager.Spawn("QuantumConsole"); }

        #endregion


        #region Command

        [Command("random_money", MonoTargetType.Single)]
        
        public void RandomMoney()
        {
            this.signalBus.Fire(new ReRandomMoneySignal());
            Debug.Log("Random money!");
        }

        [Command("vibration", MonoTargetType.Single)]
        public void TurnOnOffVibration()
        {
            this.settingDataController.SetVibrationOnOff();
            var message = this.settingDataController.IsVibrationOn ? "Vibration is turn on" : "Vibration is turn off";
            Debug.Log(message);
        }

        #endregion
    }
}