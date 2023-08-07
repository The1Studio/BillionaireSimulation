namespace TheOneStudio.HyperCasual.Scenes.Main.UI.ScreenStates.CreateCharacter
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;
    using TheOneStudio.UITemplate.UITemplate.Scenes.Utils;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;

    public class GiveCharacterNameScreenData
    {
        public Action<string> OnAcceptName;
    }

    public class GiveCharacterNameScreenView : BaseView
    {
        public TMP_InputField  inputField;
        public TextMeshProUGUI txtPlaceholder;
        public Button          btnRandomName;
        public Button          btnOk;
    }

    [ScreenInfo(nameof(GiveCharacterNameScreenView))]
    public class GiveCharacterNameScreenPresenter : UITemplateBaseScreenPresenter<GiveCharacterNameScreenView, GiveCharacterNameScreenData>
    {
        private readonly UserLocalData userLocalData;
        public GiveCharacterNameScreenPresenter(SignalBus signalBus, ILogService logger, UserLocalData userLocalData) : base(signalBus, logger) { this.userLocalData = userLocalData; }

        public override UniTask BindData(GiveCharacterNameScreenData popupModel)
        {
            this.View.txtPlaceholder.text = this.GetRandomName();
            return UniTask.CompletedTask;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();

            this.View.btnOk.onClick.AddListener(this.OnOkClicked);
            this.View.btnRandomName.onClick.AddListener(this.OnRandomNameClicked);
        }

        private void OnOkClicked()
        {
            var text = this.View.inputField.text;
            this.Model.OnAcceptName?.Invoke(text);
        }

        private void OnRandomNameClicked() { this.View.inputField.text = this.GetRandomName(); }

        private string GetRandomName()
        {
            var male     = UnityEngine.Random.Range(0, 2) % 2 == 0;
            var nameType = male ? 4 : 2;
            return NVJOBNameGen.GiveAName(nameType);
        }
    }
}