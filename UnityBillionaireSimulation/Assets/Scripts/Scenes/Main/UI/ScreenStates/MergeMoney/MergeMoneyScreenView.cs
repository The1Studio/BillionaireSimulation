namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.MergeMoney
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.Utils;
    using Sirenix.Utilities;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Random = UnityEngine.Random;

    public class MergeMoneyScreenView : BaseView
    {
        public List<SlotController> listSlotControllers = new();
        public Image                energyFill;
        public GameObject           spaceShip;
    }

    [ScreenInfo(nameof(MergeMoneyScreenView))]
    public class MergeMoneyScreenPresenter : BaseScreenPresenter<MergeMoneyScreenView>
    {
        private readonly MiscParamBlueprint miscParamBlueprint;
        private          CurrencyBlueprint  currencyBlueprint;
        public MergeMoneyScreenPresenter(SignalBus signalBus, MiscParamBlueprint miscParamBlueprint, CurrencyBlueprint currencyBlueprint) : base(signalBus)
        {
            this.miscParamBlueprint = miscParamBlueprint;
            this.currencyBlueprint  = currencyBlueprint;
        }
        public override UniTask BindData()
        {
            this.LoadRandomMoneyDataToListSlot();
            return UniTask.CompletedTask;
        }

        private void LoadRandomMoneyDataToListSlot()
        {
            var          firstExpectNumber = UnityEngine.Random.Range(2, 8);
            List<string> listMoneySelected = new();
            listMoneySelected.AddRange(this.SplitMoney("Coin_4", firstExpectNumber));
            listMoneySelected.AddRange(this.SplitMoney("Coin_4", 9-firstExpectNumber>=2? Random.Range(2,9-firstExpectNumber+1):0));
            listMoneySelected.Shuffle();
            //init empty slot
            for (var i = 0; i < this.View.listSlotControllers.Count; i++)
            {
                this.View.listSlotControllers[i].MoneySlotData = new MoneySlotData() { MoneyId = null, SlotIndex = i, SlotStatus = SlotStatus.Empty };
            }
            //load data to slot
            for (var i = 0; i < listMoneySelected.Count; i++)
            {
                this.View.listSlotControllers[i].MoneySlotData = new MoneySlotData() { MoneyId = listMoneySelected[i], SlotIndex = i, SlotStatus = SlotStatus.CanMerge };
                this.View.listSlotControllers[i].SetupSlotView();
            }
            
        }

        private List<string> SplitMoney(string moneyId, int expectNumber)
        {
            List<string> result      = new();
            if (expectNumber == 0) return result;
            result.Add(moneyId);
            while (result.Count!=expectNumber)
            {
                var moneyId1 = result[0];
                result.RemoveAt(0);
                var moneyMergeToId = this.currencyBlueprint.FirstOrDefault(x => x.Value.MergeUpTo.Equals(moneyId1)).Value.CurrencyId;
                result.Add(moneyMergeToId);
                result.Add(moneyMergeToId);
            }
            return result;
        }
    }
}