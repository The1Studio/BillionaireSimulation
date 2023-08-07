namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Others;
    using Zenject;

    public enum CharacterGender
    {
        Male,
        Female
    }

    public class CharacterModel : IBaseItemModel
    {
        public CharacterGender CharacterGender;
    }

    public class CharacterView : TViewMono
    {
    }

    public class CharacterPresenter : BaseItemPresenter<CharacterView, CharacterModel>
    {
        public CharacterPresenter(ObjectPoolManager objectPoolManager, IGameAssets gameAssets, SignalBus signalBus) : base(objectPoolManager, gameAssets, signalBus) { }
    }
}