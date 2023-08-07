namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using UnityEngine;
    using Zenject;

    public class SlotController : MonoBehaviour
    {
        [Inject] private          ObjectPoolManager objectPoolManager;
        [Inject] private readonly SignalBus         signalBus;
        public                    MoneySlotData     MoneySlotData;
        public                    Transform         topPos;
        private                   SlotItem          slotItem;
        private                   GameObject        slotItemObject;

        private void Awake()
        {
            this.GetCurrentContainer().Inject(this); 
        }
        private void Start()
        {
            this.signalBus.Subscribe<UpdateMoneyInSlotSignal>(this.UpdateSlotData);
        }

        public async void SetupSlotView()
        {
            if (this.MoneySlotData.IsEmpty) return;
            if (this.slotItemObject != null) Destroy(this.slotItemObject);

            //spawn slot item object
            this.slotItemObject                         = await this.objectPoolManager.Spawn("Item", this.transform);
            this.slotItemObject.transform.localPosition = Vector3.zero;
            this.slotItemObject.transform.localScale    = new Vector3(1, 1, 1);


            //setup slot item data
            this.slotItem                = this.slotItemObject.GetComponent<SlotItem>();
            this.slotItem.topPos         = this.topPos;
            this.slotItem.slotController = this;
            this.slotItem.UpdateData(this.MoneySlotData);
        }
        
        public void ResetSlot()
        {
            if (this.slotItemObject != null) Destroy(this.slotItemObject);
            this.MoneySlotData.MoneyId    = null;
            this.MoneySlotData.SlotStatus = SlotStatus.Empty;
            
        }
        public void OverrideMoneyData(MoneySlotData moneySlotData)
        {
            this.MoneySlotData.MoneyId    = moneySlotData.MoneyId;
            this.MoneySlotData.SlotStatus = moneySlotData.SlotStatus;
        }

        private void UpdateSlotData(UpdateMoneyInSlotSignal signal)
        {
            if (signal.MoneySlotData.SlotIndex != this.MoneySlotData.SlotIndex) return;

            if (signal.IsReset)
            {
                this.ResetSlot();
                return;
            }
            this.MoneySlotData = signal.MoneySlotData;
            this.SetupSlotView();
            
        }
    }
}