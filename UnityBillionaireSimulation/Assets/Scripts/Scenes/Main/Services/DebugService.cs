﻿namespace TheOneStudio.HyperCasual.Scenes.Main.Services
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using QFSW.QC;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using UnityEngine;
    using Zenject;

    public class DebugService : MonoBehaviour
    {
        [Inject] private ObjectPoolManager objectPoolManager;
        [Inject] private SignalBus         signalBus;
        private void Start()
        {
            this.GetCurrentContainer().Inject(this);
#if DEBUG_MODE
            this.SpawnQuantumObject();
#endif
        }

        private async void SpawnQuantumObject()
        {
            await this.objectPoolManager.Spawn("QuantumConsole");
        }
        
        [Command("random-money",MonoTargetType.Single)]
        public void RandomMoney()
        {
            this.signalBus.Fire(new ReRandomMoneySignal());
        }
    }
}