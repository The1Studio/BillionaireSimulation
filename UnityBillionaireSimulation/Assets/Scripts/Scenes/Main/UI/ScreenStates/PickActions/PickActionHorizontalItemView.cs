namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.PickActions
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using UnityEngine;
    using UnityEngine.UI;

    public class PickActionHorizontalItemView : BasePickActionItemView
    {
        public Image      imgIcon;
        public GameObject goRewardAds;
    }

    public class PickActionHorizontalItemPresenter : BasePickActionItemPresenter<BasePickActionItemView, PickActionItemModel>
    {
        private PickActionHorizontalItemView NewView => this.View as PickActionHorizontalItemView;
        public PickActionHorizontalItemPresenter(IGameAssets gameAssets) : base(gameAssets) { }

        public override async void BindData(PickActionItemModel param)
        {
            base.BindData(param);
            this.NewView.imgIcon.sprite = await this.GameAssets.LoadAssetAsync<Sprite>(param.PickActionChoiceRecord.ImageAsset);
            this.NewView.goRewardAds.SetActive(param.PickActionChoiceRecord.UnlockByAds);
        }
    }
}