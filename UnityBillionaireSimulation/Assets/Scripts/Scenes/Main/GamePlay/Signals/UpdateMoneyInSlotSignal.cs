namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Signals
{
    using TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views;

    public class UpdateMoneyInSlotSignal
    {
        public bool          IsReset = false;
        public MoneySlotData MoneySlotData;
    }
}