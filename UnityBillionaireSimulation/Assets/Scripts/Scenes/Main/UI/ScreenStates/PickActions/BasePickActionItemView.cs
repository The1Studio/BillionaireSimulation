namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.PickActions
{
    using System;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using TheOneStudio.HyperCasual.Blueprints;
    using UnityEngine.UI;

    public class PickActionItemModel
    {
        public PickActionChoiceRecord         PickActionChoiceRecord { get; set; }
        public Action<PickActionChoiceRecord> OnClick                { get; set; }
    }

    public class BasePickActionItemView : TViewMono
    {
        public Button btnSelect;
    }

    public class BasePickActionItemPresenter<TView, TModel> : BaseUIItemPresenter<TView, TModel> where TView : BasePickActionItemView where TModel : PickActionItemModel
    {
        public PickActionItemModel Model { get; set; }

        public BasePickActionItemPresenter(IGameAssets gameAssets) : base(gameAssets) { }

        public override void BindData(TModel param)
        {
            this.Model = param;
            this.View.btnSelect.onClick.AddListener(() => this.Model.OnClick?.Invoke(this.Model.PickActionChoiceRecord));
        }

        public override void Dispose()
        {
            base.Dispose();
            this.View.btnSelect.onClick.RemoveAllListeners();
        }
    }
}