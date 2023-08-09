namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.MergeMoney
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using GameFoundation.Scripts.Utilities.Utils;
    using Mono.CSharp;
    using QFSW.QC;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using TheOneStudio.HyperCasual.Scenes.Main.UI.Extension;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    public class MergeMoneyScreenView : BaseView
    {
        public List<SlotController> listSlotControllers = new();
        public GameObject           energyObject;
        public Image                energyFill;
        public GameObject           spaceShip;
        public GameObject           mergeField;
        public GameObject           topPos;
        public GameObject           misPos;
        public GameObject           vfxSpawn;
        public GameObject           fingerObject;
    }
    

    [ScreenInfo(nameof(MergeMoneyScreenView))]
    public class MergeMoneyScreenPresenter : BaseScreenPresenter<MergeMoneyScreenView>
    {
        private readonly MiscParamBlueprint miscParamBlueprint;
        private          CurrencyBlueprint  currencyBlueprint;
        private          int                targetMoney;
        private          int                currentMoney = 0;
        private readonly IAudioService      audioService;
        private          ObjectPoolManager  objectPoolManager;
        private          UserLocalData      userLocalData;
        public MergeMoneyScreenPresenter(SignalBus signalBus, MiscParamBlueprint miscParamBlueprint, CurrencyBlueprint currencyBlueprint,IAudioService audioService,ObjectPoolManager objectPoolManager,UserLocalData userLocalData) : base(signalBus)
        {
            this.miscParamBlueprint = miscParamBlueprint;
            this.currencyBlueprint  = currencyBlueprint;
            this.audioService       = audioService;
            this.objectPoolManager  = objectPoolManager;
            this.userLocalData      = userLocalData;
        }
        public override UniTask BindData()
        {
            this.LoadRandomMoneyDataToListSlot();
            this.SubscribeSignal();
            this.Setup();
            return UniTask.CompletedTask;
        }

        private void Setup()
        {
            this.View.energyObject.SetActive(true);
            this.currentMoney               = 0;
            this.View.energyFill.fillAmount = 0;
            this.View.energyObject.transform.localPosition = Vector3.zero;
            this.View.energyObject.transform.localScale  = Vector3.one;
            var     width   = this.View.mergeField.GetComponent<RectTransform>().rect.width;
            var     height  = this.View.mergeField.GetComponent<RectTransform>().rect.height;
            Vector2 newSize = new Vector2(width / 3f, height /3f);
            this.View.mergeField.GetComponent<GridLayoutGroup>().cellSize = newSize;
        }

        private void SubscribeSignal()
        {
            this.SignalBus.Subscribe<MergeCompleteSignal>(this.DoEffectMoneyFlyToEnergy);
            this.SignalBus.Subscribe<ReRandomMoneySignal>(this.ReRandomMoney);
            this.SignalBus.Subscribe<CompleteTutorialSignal>(this.CompleteTut);
        }

        private void ReRandomMoney()
        {
            this.Setup();
            this.View.listSlotControllers.ForEach(e=>e.ResetSlot());
            this.LoadRandomMoneyDataToListSlot();
        }

        private void CompleteTut()
        {
            this.userLocalData.IsFirstTime = false;
            this.View.fingerObject.SetActive(false);
        }

        private void UnSubscribeSignal()
        {
            this.SignalBus.TryUnsubscribe<MergeCompleteSignal>(this.DoEffectMoneyFlyToEnergy);
            this.SignalBus.TryUnsubscribe<ReRandomMoneySignal>(this.ReRandomMoney);
            this.SignalBus.TryUnsubscribe<CompleteTutorialSignal>(this.CompleteTut);
        }

        private void DoEffectMoneyFlyToEnergy(MergeCompleteSignal signal)
        {
            var slotItemTransform = signal.SlotController.slotItemObject.transform;
            slotItemTransform.SetParent(this.View.topPos.transform);
            Sequence sequence = DOTween.Sequence();
            
            //move to middle and scale
            sequence.Append(slotItemTransform.DOMove(this.View.misPos.transform.position, 0.4f).SetEase(Ease.OutQuad));
            var scaleTween = slotItemTransform.DOScale(new Vector3(2, 2, 2), 0.4f).SetEase(Ease.OutQuad);
            sequence.Join(scaleTween);
            scaleTween.onComplete += () =>
            {
                var vfx = Object.Instantiate(this.View.vfxSpawn, this.View.misPos.transform);
                vfx.transform.localPosition = Vector3.zero;
            };
            
            //sleep and play sound
            sequence.AppendInterval(0.7f);
            this.audioService.PlaySound(this.miscParamBlueprint.CompleteMergeSound);
            
            
            //move to energy field
            sequence.Append(slotItemTransform.DOJump(this.View.energyFill.transform.position, 5f, 1, 0.5f));
            sequence.Join(slotItemTransform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f).SetEase(Ease.OutQuad));
            sequence.onComplete += () =>
            {
                this.currentMoney += this.currencyBlueprint[signal.SlotController.MoneySlotData.MoneyId].Value;
                var newEnergyValue = this.currentMoney * 1.0f / this.targetMoney;
                this.audioService.PlaySound(this.miscParamBlueprint.FuelSound);
                this.View.energyFill.DoFillAmount(newEnergyValue).SetEase(Ease.OutQuad);
                if (newEnergyValue>=1)
                {
                    this.DoEffectFullOfEnergy();
                }
                Object.Destroy(signal.SlotController.slotItemObject);
                signal.SlotController.OverrideMoneyData(new MoneySlotData(){MoneyId = null,SlotStatus = SlotStatus.Empty});
            };
            
        }

        private void DoEffectFullOfEnergy()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.View.energyFill.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo));
            sequence.Append(this.View.energyObject.transform.DOJump(this.View.spaceShip.transform.position, 50, 1, 0.7f).SetEase(Ease.OutQuad));
            sequence.Join(this.View.energyObject.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.7f).SetEase(Ease.OutQuad));
            sequence.onComplete += () =>
            {
                this.audioService.PlaySound(this.miscParamBlueprint.CompleteMergeSound);
                this.SignalBus.Fire(new CompleteMergeGameSignal());
                this.View.energyObject.SetActive(false);
            };
        }
        
        private async void LoadRandomMoneyDataToListSlot()
        {
            var          firstExpectNumber = Random.Range(2, 8);
            List<string> listMoneySelected = new();
            listMoneySelected.AddRange(this.SplitMoney("Coin_4", firstExpectNumber));
            listMoneySelected.AddRange(this.SplitMoney("Coin_4", 9 - firstExpectNumber >= 2 ? Random.Range(2, 9 - firstExpectNumber + 1) : 0));
            this.targetMoney = 9 - firstExpectNumber >= 2 ? 40 : 20;
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

            if(!this.userLocalData.IsFirstTime) return;
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            this.DoTutorial();
        }

        private void DoTutorial()
        {
            var listItemSameValue = this.FindTwoSlotSameValue();
            if(listItemSameValue==null) return;
            this.View.fingerObject.SetActive(true);
            var sequence = DOTween.Sequence();
            this.View.fingerObject.transform.position = listItemSameValue[0].slotItemObject.transform.position;
            sequence.Append(this.View.fingerObject.transform.DOMove(listItemSameValue[1].slotItemObject.transform.position, 1.5f));
            sequence.SetLoops(-1, LoopType.Restart);
        }

        private List<SlotController> FindTwoSlotSameValue()
        {
            foreach (var item in this.View.listSlotControllers)
            {
                var listSameValue = this.View.listSlotControllers.Where(e =>!e.MoneySlotData.IsEmpty && e.MoneySlotData.MoneyId.Equals(item.MoneySlotData.MoneyId)).ToList();
                if (listSameValue.Count >= 2)
                {
                    return listSameValue.Take(2).ToList();
                }
            }
            return null;
        }

        private List<string> SplitMoney(string moneyId, int expectNumber)
        {
            List<string> result = new();
            if (expectNumber == 0) return result;
            result.Add(moneyId);
            while (result.Count != expectNumber)
            {
                var moneyId1 = result[0];
                result.RemoveAt(0);
                var moneyMergeToId = this.currencyBlueprint.FirstOrDefault(x => x.Value.MergeUpTo.Equals(moneyId1)).Value.CurrencyId;
                result.Add(moneyMergeToId);
                result.Add(moneyMergeToId);
            }

            return result;
        }
        public override void Dispose()
        {
            base.Dispose();
            this.UnSubscribeSignal();
        }
    }
}