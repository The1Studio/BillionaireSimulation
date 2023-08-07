namespace TheOneStudio.HyperCasual.Models
{
    using Zenject;

    public class LevelResult
    {
        #region inject

        private readonly SignalBus signalBus;

        #endregion

        public LevelResult(SignalBus signalBus) { this.signalBus = signalBus; }

        public void ResetCache() {  }
    }
}