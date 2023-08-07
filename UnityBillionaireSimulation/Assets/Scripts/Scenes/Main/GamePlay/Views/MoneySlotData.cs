namespace TheOneStudio.HyperCasual.Scenes.Main.GamePlay.Views
{
    public class MoneySlotData
    {
        public int        SlotIndex;
        public string     MoneyId;
        public SlotStatus SlotStatus;
        public bool       IsEmpty => this.SlotStatus == SlotStatus.Empty;
        
        public void ResetData()
        {
            this.MoneyId    = null;
            this.SlotStatus = SlotStatus.Empty;
        }
    }
    
    public enum SlotStatus
    {
        Empty,
        CanMerge
    }
}