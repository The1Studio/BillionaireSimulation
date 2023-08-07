namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.PickActions
{
    using GameFoundation.Scripts.AssetLibrary;
    using TMPro;
    using UnityEngine;

    public class PickActionVerticalItemView : BasePickActionItemView
    {
        public TextMeshProUGUI txtContent;
        public GameObject      goRewardAds;
    }

    public class PickActionVerticalItemPresenter : BasePickActionItemPresenter<BasePickActionItemView, PickActionItemModel>
    {
        private new PickActionVerticalItemView NewView => this.View as PickActionVerticalItemView;
        public PickActionVerticalItemPresenter(IGameAssets gameAssets) : base(gameAssets) { }

        public override async void BindData(PickActionItemModel param)
        {
            base.BindData(param);
            this.NewView.txtContent.text = param.PickActionChoiceRecord.Content;
            this.NewView.goRewardAds.SetActive(param.PickActionChoiceRecord.UnlockByAds);
        }
    }
}