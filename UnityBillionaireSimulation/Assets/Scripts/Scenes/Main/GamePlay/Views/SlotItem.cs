﻿namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Zenject;

    public class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform   rectTransform;
        private Vector2         currentAnchoredPos;
        private Vector3         currentPos;
        private Image           image;
        public  TextMeshProUGUI moneyValue;
        public  bool            isMerging = false;

        public                    Vector3                         offsetLeft  = new Vector3(90f, 70f, 0);
        public                    Vector3                         offsetRight = new Vector3(-90f, 70f, 0);
        public                    Vector3                         offsetTop   = new Vector3(0, 70f, 0);
        [Inject] private          EventSystem                     eventSystem;
        [Inject] private          IGameAssets                     gameAssets;
        [Inject] private          SignalBus                       signalBus;
        [Inject] private          ObjectPoolManager               objectPoolManager;
        [Inject] private          CurrencyBlueprint               currencyBlueprint;
        [Inject] private readonly IAudioService                   audioService;
        [Inject] private readonly MiscParamBlueprint              miscParamBlueprint;
        [Inject] private readonly UITemplateSettingDataController uiTemplateSettingDataController;

        private PointerEventData pointerEventData;
        public  Transform        topPos;
        public  Transform        currentParent;
        public  SlotController   slotController;
        private void Awake()
        {
            this.GetCurrentContainer().Inject(this);
            this.rectTransform = this.GetComponent<RectTransform>();
            this.image         = this.GetComponent<Image>();
        }

        #region DragHandle

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.image.raycastTarget = false;
            this.currentAnchoredPos  = this.rectTransform.anchoredPosition;
            this.currentParent       = this.transform.parent;
            this.currentPos          = this.transform.position;
            this.transform.SetParent(this.topPos);
            this.signalBus.Fire(new CompleteTutorialSignal());
        }
        public void OnDrag(PointerEventData eventData)    { this.transform.position = Vector3.Lerp(this.transform.position, Input.mousePosition + this.offsetTop, 0.6f); }
        public void OnEndDrag(PointerEventData eventData) { this.HandlePositionOfSlot(); }

        private void HandlePositionOfSlot()
        {
            List<RaycastResult> results = new();
            this.pointerEventData = new PointerEventData(this.eventSystem)
            {
                position = this.transform.position
            };

            EventSystem.current.RaycastAll(this.pointerEventData, results);
            if (!results.Any(e => e.gameObject.CompareTag("Slot")))
            {
                this.SetItemReturnBack();
                return;
            }

            var itemSlot = results.FirstOrDefault(e => e.gameObject.CompareTag("Slot")).gameObject;
            this.HandleLogicItemSlot(itemSlot);
        }

        #endregion


        private void HandleLogicItemSlot(GameObject slotItem)
        {
            if (slotItem.GetComponent<SlotController>().MoneySlotData.IsEmpty)
            {
                this.UpdateItemToNewSlot(slotItem);
                return;
            }

            this.MergeTwoSlotItem(slotItem.GetComponent<SlotController>().slotItemObject);
        }

        private void SetItemReturnBack()
        {
            this.transform.DOMove(this.currentPos, 0.5f).SetEase(Ease.OutQuad).onComplete += () =>
            {
                this.transform.SetParent(this.currentParent);
                this.rectTransform.anchoredPosition = this.currentAnchoredPos;
                this.image.raycastTarget            = true;
            };
        }


        #region UpdateViewAndData

        private void UpdateItemToNewSlot(GameObject slotObject)
        {
            if (!slotObject.GetComponent<SlotController>().MoneySlotData.IsEmpty)
            {
                this.SetItemReturnBack();
                return;
            }

            slotObject.GetComponent<SlotController>().OverrideMoneyData(this.slotController.MoneySlotData);
            slotObject.GetComponent<SlotController>().SetupSlotView();
            this.slotController.ResetSlot();
            this.image.raycastTarget = true;
        }
        public async void UpdateData(MoneySlotData moneySlotData)
        {
            var moneyData = this.currencyBlueprint[moneySlotData.MoneyId];
            this.image.sprite    = await this.gameAssets.LoadAssetAsync<Sprite>(moneyData.CurrencyId);
            this.moneyValue.text = "$" + moneyData.Value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region Merge

        private void MergeTwoSlotItem(GameObject itemObject)
        {
            var firstSlotData  = itemObject.gameObject.GetComponent<SlotItem>().slotController.MoneySlotData;
            var secondSlotData = this.slotController.MoneySlotData;
            var nextMoneyId    = this.currencyBlueprint[firstSlotData.MoneyId].MergeUpTo;
            if (firstSlotData.MoneyId != secondSlotData.MoneyId || string.IsNullOrEmpty(nextMoneyId) || firstSlotData.SlotIndex == secondSlotData.SlotIndex)
            {
                if (firstSlotData.SlotIndex != secondSlotData.SlotIndex)
                {
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(itemObject.transform.DORotate(new Vector3(0, 0, 15), 0.1f).SetEase(Ease.OutQuad));
                    sequence.Append(itemObject.transform.DORotate(new Vector3(0, 0, -15), 0.1f).SetEase(Ease.OutQuad));
                    sequence.Append(itemObject.transform.DORotate(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.OutQuad));
                    sequence.SetLoops(2, LoopType.Restart);
                }

                this.SetItemReturnBack();
                return;
            }

            if (this.isMerging|| itemObject.gameObject.GetComponent<SlotItem>().isMerging)
            {
                this.SetItemReturnBack();
                return;
            }
            this.isMerging                                           = true;
            itemObject.gameObject.GetComponent<SlotItem>().isMerging = true;
            
            firstSlotData.MoneyId                                    = nextMoneyId;
            this.DoMerge(itemObject, this.gameObject, firstSlotData, () =>
            {
                this.signalBus.Fire(new UpdateMoneyInSlotSignal() { MoneySlotData = firstSlotData });
                this.signalBus.Fire(new UpdateMoneyInSlotSignal() { MoneySlotData = secondSlotData, IsReset = true });
                this.image.raycastTarget                                 = true;
                this.isMerging                                           = false;
                itemObject.gameObject.GetComponent<SlotItem>().isMerging = false;
            });
        }

        private void DoMerge(GameObject firstItem, GameObject secondItem, MoneySlotData newSlotData, Action onCompleteAction)
        {
            var      position       = firstItem.transform.position;
            var      firstPos       = position + this.offsetLeft;
            var      secondPos      = position + this.offsetRight;
            var      finalPos       = position + this.offsetTop;
            Sequence sequence       = DOTween.Sequence();
            Sequence secondSequence = DOTween.Sequence();
            sequence.Join(firstItem.transform.DOMove(firstPos, 0.4f)).SetEase(Ease.OutQuad);
            sequence.Join(secondItem.transform.DOMove(secondPos, 0.4f)).SetEase(Ease.OutQuad);
            secondSequence.Join(firstItem.transform.DOMove(finalPos, 0.25f)).SetEase(Ease.OutQuad);
            secondSequence.Join(secondItem.transform.DOMove(finalPos, 0.25f)).SetEase(Ease.OutQuad);
            sequence.Append(secondSequence);

            //vibrate and play sound
            if (this.uiTemplateSettingDataController.IsVibrationOn) Handheld.Vibrate();
            this.audioService.PlaySound(this.miscParamBlueprint.MergeSound);

            sequence.onComplete += () =>
            {
                this.SpawnVfx(this.currencyBlueprint[newSlotData.MoneyId].VfxMergeComplete, firstItem.transform);
                firstItem.GetComponent<SlotItem>().UpdateData(newSlotData);
                secondItem.gameObject.SetActive(false);
                firstItem.transform.DOMove(position, 0.2f).SetEase(Ease.OutQuad).onComplete += () => { onCompleteAction?.Invoke(); };
            };
        }

        private async void SpawnVfx(string vfxName, Transform parent)
        {
            var vfxObject = await this.objectPoolManager.Spawn(vfxName, this.topPos);
            vfxObject.transform.position = parent.transform.position;
        }

        #endregion
    }
}