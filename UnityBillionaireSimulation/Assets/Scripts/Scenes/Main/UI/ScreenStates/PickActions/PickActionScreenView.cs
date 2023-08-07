namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.PickActions
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Utils;
    using UnityEngine;
    using Zenject;

    public class PickActionScreenModel
    {
        public string                         Id            { get; set; }
        public Action<PickActionChoiceRecord> OnClickOption { get; set; }
    }

    public class PickActionScreenView : BaseView
    {
        public GameObject horizontalView;
        public GameObject verticalView;
        public Transform  itemHorizontalContainer;
        public Transform  itemVerticalContainer;
    }

    [ScreenInfo(nameof(PickActionScreenView))]
    public class PickActionScreenPresenter : UITemplateBaseScreenPresenter<PickActionScreenView, PickActionScreenModel>
    {
        private List<BasePickActionItemPresenter<BasePickActionItemView, PickActionItemModel>> pickActionItemPresenters = new();
        private List<PickActionItemModel>                                                      pickActionItemModels     = new();

        private readonly SegmentPickActionBlueprint pickActionBlueprint;
        private readonly DiContainer                container;
        private readonly IGameAssets                gameAssets;
        private readonly ObjectPoolManager          objectPoolManager;

        public PickActionScreenPresenter(SignalBus signalBus, ILogService logger, SegmentPickActionBlueprint pickActionBlueprint, DiContainer container, IGameAssets gameAssets,
            ObjectPoolManager objectPoolManager) : base(signalBus, logger)
        {
            this.pickActionBlueprint = pickActionBlueprint;
            this.container           = container;
            this.gameAssets          = gameAssets;
            this.objectPoolManager   = objectPoolManager;
        }

        public override async UniTask BindData(PickActionScreenModel popupScreenModel) { this.InitItems(); }

        private void RecycleItems()
        {
            for (var i = this.View.itemHorizontalContainer.childCount - 1; i >= 0; i--)
            {
                this.View.itemHorizontalContainer.GetChild(i).Recycle();
            }

            for (var i = this.View.itemVerticalContainer.childCount - 1; i >= 0; i--)
            {
                this.View.itemVerticalContainer.GetChild(i).Recycle();
            }
        }

        private async void InitItems()
        {
            var segmentPickActionRecord = this.pickActionBlueprint.GetDataById(this.Model.Id);
            this.pickActionItemPresenters.Clear();
            this.pickActionItemModels.Clear();
            this.RecycleItems();
            this.View.horizontalView.SetActive(segmentPickActionRecord.PickActionType == PickActionType.Horizontal);
            this.View.verticalView.SetActive(segmentPickActionRecord.PickActionType == PickActionType.Vertical);

            var taskViews  = new List<UniTask<BasePickActionItemView>>();
            var viewName   = segmentPickActionRecord.PickActionType == PickActionType.Horizontal ? nameof(PickActionHorizontalItemView) : nameof(PickActionVerticalItemView);
            var itemParent = segmentPickActionRecord.PickActionType == PickActionType.Horizontal ? this.View.itemHorizontalContainer : this.View.itemVerticalContainer;

            foreach (var item in segmentPickActionRecord.ChoiceRecords)
            {
                this.pickActionItemPresenters.Add(segmentPickActionRecord.PickActionType == PickActionType.Horizontal
                    ? this.container.Instantiate<PickActionHorizontalItemPresenter>()
                    : this.container.Instantiate<PickActionVerticalItemPresenter>());

                this.pickActionItemModels.Add(new PickActionItemModel
                {
                    PickActionChoiceRecord = item,
                    OnClick                = this.OnClick
                });
                taskViews.Add(this.objectPoolManager.Spawn<BasePickActionItemView>(viewName));
            }

            var views = await UniTask.WhenAll(taskViews);

            for (int i = 0; i < this.pickActionItemPresenters.Count; i++)
            {
                views[i].transform.SetParent(itemParent, false);
                this.pickActionItemPresenters[i].SetView(views[i]);
                this.pickActionItemPresenters[i].BindData(this.pickActionItemModels[i]);
            }
        }

        private void OnClick(PickActionChoiceRecord obj) { this.Model.OnClickOption?.Invoke(obj); }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var presenter in this.pickActionItemPresenters)
            {
                presenter.Dispose();
            }
        }
    }
}