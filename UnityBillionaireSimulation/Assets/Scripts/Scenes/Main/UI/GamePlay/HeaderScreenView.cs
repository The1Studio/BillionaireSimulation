namespace TheOneStudio.HyperCasual.Scenes.Main.UI.GamePlay
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;

    public class HeaderScreenModel
    {
    }

    public class HeaderScreenView : TViewMono
    {
        public TextMeshProUGUI txtTitle;
    }

    public class HeaderScreenPresenter : BaseUIItemPresenter<HeaderScreenView, HeaderScreenModel>
    {
        private readonly SignalBus   signalBus;
        private readonly LevelHelper levelHelper;

        public HeaderScreenPresenter(IGameAssets gameAssets, SignalBus signalBus, LevelHelper levelHelper) : base(gameAssets)
        {
            this.signalBus   = signalBus;
            this.levelHelper = levelHelper;
        }

        public override void BindData(HeaderScreenModel param)
        {
            this.signalBus.Subscribe<ResetGameplaySignal>(this.OnUpdateUI);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.signalBus.Unsubscribe<ResetGameplaySignal>(this.OnUpdateUI);
        }

        private void OnUpdateUI()
        {
            var mapLevelRecord = this.levelHelper.GetMapLevelRecordByLevel();
            this.View.txtTitle.text = mapLevelRecord.Name;
        }
    }
}