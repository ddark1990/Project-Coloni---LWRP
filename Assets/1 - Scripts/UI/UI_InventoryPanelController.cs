using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_InventoryPanelController : MonoBehaviour
    {
        //prof. rework incoming, split into static and dynamic sections, and separate each 
        [SerializeField] private List<UI_InventorySlot> inventorySlots;
        [SerializeField] private List<UI_EquipmentSlot> equipmentSlots;

        private AiController _controller;
        
        public void UpdateInventoryUi(Item item, bool active)
        {
            if (SelectionManager.Instance.currentlySelectedObject == null || SelectionManager.Instance.currentlySelectedObject as AiController == null) return;

            _controller = SelectionManager.Instance.currentlySelectedObject as AiController;

            if(active)
                ActivateInventorySlotData(item, _controller);
            else
                ClearInventorySlotData(item, _controller);
        }
        private void ClearInventorySlotData(Item item, AiController controller)
        {
            if (controller.inventory.holdingItems.Count.Equals(0))
            {
                foreach (var slot in inventorySlots)
                {
                    if(slot.isActiveAndEnabled)
                        slot.gameObject.SetActive(false);
                }
                return;
            }
            
            foreach (var slot in inventorySlots)
            {
                if (slot.itemReference != item) continue;
                
                slot.active = false;

                slot.itemReference = null;
                    
                /*slot.itemName.text = string.Empty;
                slot.itemDescription.text = string.Empty;
                slot.itemCount.text = string.Empty;
                    
                slot.itemWeight.text = string.Empty;
                    
                slot.itemIcon.sprite = null;*/

                //SetCanvasGroupSettings(slot.canvasGroup, 0, false);
                slot.gameObject.SetActive(false);
            }
        }
        
        private void ActivateInventorySlotData(Item item, AiController controller)
        {
            foreach (var slot in inventorySlots)
            {
                if (!slot.active)
                {
                    slot.active = true;

                    slot.itemReference = item;
                    
                    slot.itemName.text = item.baseObjectInfo.ObjectName;
                    slot.itemDescription.text = item.baseObjectInfo.Description;
                    slot.itemCount.text = UI_SelectionController.CachedIntToString[item.itemCount];
                    
                    slot.itemWeight.text = UI_SelectionController.CachedIntToString[(int)item.itemTypeData.itemData.itemWeight];
                    
                    slot.itemIcon.sprite = item.itemTypeData.itemData.inventorySprite;
                    
                    slot.dropButton.onClick.AddListener(delegate { controller.inventory.DropItem(item); });

                    var useCanvasGroup = slot.useButton.GetComponent<CanvasGroup>();
                    var dropCanvasGroup = slot.dropButton.GetComponent<CanvasGroup>();
                    var equipCanvasGroup = slot.equipButton.GetComponent<CanvasGroup>();
                    
                    switch (item.itemTypeData)
                    {
                        case Consumable consumable:
                            SetCanvasGroupSettings(useCanvasGroup, 1, true);
                            SetCanvasGroupSettings(dropCanvasGroup, 1, true);
                            SetCanvasGroupSettings(equipCanvasGroup, 0, false);
                            
                            break;
                        case Resource resource:
                            SetCanvasGroupSettings(useCanvasGroup, 0, false);
                            SetCanvasGroupSettings(dropCanvasGroup, 1, true);
                            SetCanvasGroupSettings(equipCanvasGroup, 0, false);

                            break;
                        case Weapon weapon:
                            SetCanvasGroupSettings(useCanvasGroup, 0, false);
                            SetCanvasGroupSettings(dropCanvasGroup, 1, true);
                            SetCanvasGroupSettings(equipCanvasGroup, 1, true);
                            
                            slot.equipButton.onClick.AddListener(delegate { controller.equipment.EquipItem(item); });
                            
                            break;
                    }

                    //SetCanvasGroupSettings(slot.canvasGroup, 1, true);
                    slot.gameObject.SetActive(true);

                    break;
                }
            }
        }
        
        public void UpdateEquipmentUi(Item item, EquipmentSlot equipmentSlot, bool active)
        {
            if (SelectionManager.Instance.currentlySelectedObject == null || SelectionManager.Instance.currentlySelectedObject as AiController == null) return;

            var controller = SelectionManager.Instance.currentlySelectedObject as AiController;

            if(active)
                ActivateEquipmentSlotData(item, controller, equipmentSlot);
            else 
                ClearEquipmentSlotData(item);
        }

        private CanvasGroup _dropButtonCanvasGroup;
        private CanvasGroup _unEquipButtonCanvasGroup;
        private void ActivateEquipmentSlotData(Item item, AiController controller, EquipmentSlot equipmentSlot)
        {
            foreach (var slot in equipmentSlots)
            {
                _dropButtonCanvasGroup = slot.dropButton.GetComponent<CanvasGroup>();
                _unEquipButtonCanvasGroup = slot.unEquipButton.GetComponent<CanvasGroup>();

                if (slot.equipmentType.Equals(item.itemTypeData.itemData.equipmentType))
                {
                    slot.unEquipButton.onClick.RemoveAllListeners();
                    slot.dropButton.onClick.RemoveAllListeners();
                    
                    slot.slotReference = equipmentSlot;
                    slot.icon.sprite = item.itemTypeData.itemData.inventorySprite;
                    slot.icon.color = new Color(1,1,1,1);
                    
                    SetCanvasGroupSettings(_dropButtonCanvasGroup, 1, true);
                    SetCanvasGroupSettings(_unEquipButtonCanvasGroup, 1, true);
                    
                    slot.unEquipButton.onClick.AddListener(delegate { controller.equipment.UnEquipItem(item); });
                    slot.dropButton.onClick.AddListener(delegate { controller.inventory.DropEquippedItem(item); });
                    
                }
            }
        }
        public void ClearEquipmentSlotData(Item item)
        {
            foreach (var slot in equipmentSlots)
            {
                if (slot.equipmentType.Equals(item.itemTypeData.itemData.equipmentType))
                {
                    slot.slotReference = null;
                    slot.icon.sprite = GameManager.Instance.globalSpriteContainer.spriteCollection["Question"];
                    slot.icon.color = new Color(1,1,1,.5f);
                    
                    SetCanvasGroupSettings(_dropButtonCanvasGroup, 0, false);
                    SetCanvasGroupSettings(_unEquipButtonCanvasGroup, 0, false);
                }
            }
        }
        
        private void SetCanvasGroupSettings(CanvasGroup canvasGroup, float alpha, bool interactive)
        {
            canvasGroup.alpha = alpha;
            canvasGroup.interactable = interactive;
        }
    }
}
