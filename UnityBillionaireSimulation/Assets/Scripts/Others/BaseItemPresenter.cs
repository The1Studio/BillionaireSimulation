namespace TheOneStudio.HyperCasual.Others
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;
    using Zenject;

    public interface IBaseItemModel
    {
    }

    public abstract class BaseItemPresenter<TView, TModel> : IDisposable where TView : MonoBehaviour where TModel : IBaseItemModel
    {
        #region inject

        protected readonly ObjectPoolManager ObjectPoolManager;
        protected readonly IGameAssets       GameAssets;
        protected readonly SignalBus         SignalBus;

        #endregion

        public TView  View  { get; protected set; }
        public TModel Model { get; protected set; }

        protected BaseItemPresenter(ObjectPoolManager objectPoolManager, IGameAssets gameAssets, SignalBus signalBus)
        {
            this.ObjectPoolManager = objectPoolManager;
            this.GameAssets        = gameAssets;
            this.SignalBus         = signalBus;
        }

        public virtual void BindData(TModel model) { this.Model = model; }

        public virtual async UniTask SetView(TView view) { this.View = view; }

        public virtual void Dispose() { }
    }

    public abstract class BaseItemPresenter<TModel> : IDisposable where TModel : IBaseItemModel
    {
        #region inject

        protected readonly ObjectPoolManager ObjectPoolManager;
        private readonly   IGameAssets       gameAssets;
        protected readonly SignalBus         SignalBus;

        #endregion

        public TModel Model { get; private set; }

        protected BaseItemPresenter(ObjectPoolManager objectPoolManager, IGameAssets gameAssets, SignalBus signalBus)
        {
            this.ObjectPoolManager = objectPoolManager;
            this.gameAssets        = gameAssets;
            this.SignalBus         = signalBus;
        }

        public virtual void BindData(TModel model) { this.Model = model; }

        public virtual void Dispose() { }
    }
}