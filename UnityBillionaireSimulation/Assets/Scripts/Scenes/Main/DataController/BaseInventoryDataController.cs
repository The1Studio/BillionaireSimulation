namespace TheOneStudio.HyperCasual.Scenes.Main.DataController
{
    using System.Collections.Generic;
    using System.Linq;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.UITemplate.UITemplate.Models;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using UnityEngine;

    public abstract class BaseInventoryDataController
    {
        #region Inject

        protected readonly UITemplateInventoryDataController InventoryDataController;
        protected readonly UserLocalData                     UserLocalData;

        protected BaseInventoryDataController(UITemplateInventoryDataController inventoryDataController, UserLocalData userLocalData)
        {
            this.InventoryDataController = inventoryDataController;
            this.UserLocalData           = userLocalData;
        }

        #endregion

        public virtual string CategoryName { get; set; }

        public string CurrentUsedItemId
        {
            get => this.InventoryDataController.GetCurrentItemSelected(this.CategoryName);
            set => this.InventoryDataController.UpdateCurrentSelectedItem(this.CategoryName, value);
        }

        public List<UITemplateItemData> GetAllItemData() { return this.InventoryDataController.GetAllItem(this.CategoryName); }

        public List<UITemplateItemData> GetAllNotOwnedItemData() => this.GetAllItemData().Where(e => e.CurrentStatus != UITemplateItemData.Status.Owned).ToList();

        public void SetOwnItem(string itemId)
        {
            if (this.InventoryDataController.TryGetItemData(itemId, out var itemData))
            {
                if (itemData.CurrentStatus == UITemplateItemData.Status.Owned) return;
                itemData.CurrentStatus = UITemplateItemData.Status.Owned;
                this.InventoryDataController.AddItemData(itemData);
            }
            else
            {
                Debug.LogError($"dmplog: {itemId} is not exist");
            }
        }

        public void SetUseItem(string itemId)
        {
            if (this.InventoryDataController.TryGetItemData(itemId, out var itemData))
            {
                if (itemData.CurrentStatus == UITemplateItemData.Status.Owned)
                {
                    this.CurrentUsedItemId = itemId;
                }
                else
                {
                    Debug.LogError($"dmplog: {itemId} is not owned");
                }
            }
            else
            {
                Debug.LogError($"dmplog: {itemId} is not exist");
            }
        }
    }
}