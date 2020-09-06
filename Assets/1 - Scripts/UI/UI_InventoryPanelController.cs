using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_InventoryPanelController : MonoBehaviour
    {
        [SerializeField] private List<UI_InventorySlot> inventorySlots;
        
        public void UpdateInventoryUi(Item item)
        {
            if (SelectionManager.Instance.currentlySelectedObject == null || SelectionManager.Instance.currentlySelectedObject as AiController == null) return;

            var colonist = SelectionManager.Instance.currentlySelectedObject as AiController;

            ActivateSlotSetData(item);
        }

        private void ActivateSlotSetData(Item item)
        {
            foreach (var slot in inventorySlots)
            {
                if (!slot.active)
                {
                    slot.gameObject.SetActive(true);
                    slot.active = true;

                    slot.itemName.text = item.baseObjectInfo.ObjectName;
                    slot.itemDescription.text = item.baseObjectInfo.Description;
                    slot.itemCount.text = UI_SelectionController.CachedIntToString[item.itemCount];
                    slot.itemIcon.sprite = item.itemData.itemData.inventorySprite;
                    
                    break;
                }
            }
        }
    }
}
