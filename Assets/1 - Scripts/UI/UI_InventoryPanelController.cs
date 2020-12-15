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


        [SerializeField] private List<UI_InventorySlot> tempInventorySlots;

        [SerializeField] private GameObject inventorySlotPrefab;
        [SerializeField] private GameObject inventoryHolderContent;

        public void PopulateInventoryData(AiController controller)
        {
            ClearInventoryUISlots();
            
            foreach (var item in controller.inventory.holdingItems)
            {
                var slot = Instantiate(inventorySlotPrefab, inventoryHolderContent.transform).GetComponent<UI_InventorySlot>();
                
                tempInventorySlots.Add(slot);
                slot.active = true;

                slot.itemReference = item.Value;
                    
                slot.itemName.text = item.Value.baseObjectInfo.ObjectName;
                slot.itemDescription.text = item.Value.baseObjectInfo.Description;
                slot.itemCount.text = UI_SelectionController.CachedIntToString[item.Value.ItemCount];
                    
                slot.itemWeight.text = UI_SelectionController.CachedIntToString[(int)item.Value.itemTypeData.itemData.itemWeight];
                    
                slot.itemIcon.sprite = item.Value.itemTypeData.itemData.inventorySprite;
                    
                slot.dropButton.onClick.AddListener(delegate { EventRelay.OnItemDropped(controller, item.Value); });
                
                switch (item.Value.itemTypeData)
                {
                    case Consumable consumable:
                            
                        break;
                    case Resource resource:

                        break;
                    case Weapon weapon:
                            
                        slot.equipButton.onClick.AddListener(delegate { EventRelay.OnItemEquipped(controller, item.Value); });
                            
                        break;
                }
            }
        }

        public void ClearInventoryUISlots()
        {
            if (tempInventorySlots.Count == 0) return;
            
            foreach (var slot in tempInventorySlots)
            {
                Destroy(slot.gameObject);
            }
            
            tempInventorySlots.Clear();
        }
        
        /*
        #region #1
        public void UpdateInventoryUi(AiController controller, Item item, bool active)
        {
            if (SelectionManager.CurrentlySelectedObject == null || SelectionManager.CurrentlySelectedObject as AiController == null) return;
            
            if(active)
                ActivateInventorySlotData(item, controller);
            else
                ClearInventorySlotData(item, controller);
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
                    
                slot.itemIcon.sprite = null;#1#

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
                    slot.itemCount.text = UI_SelectionController.CachedIntToString[item.ItemCount];
                    
                    slot.itemWeight.text = UI_SelectionController.CachedIntToString[(int)item.itemTypeData.itemData.itemWeight];
                    
                    slot.itemIcon.sprite = item.itemTypeData.itemData.inventorySprite;
                    
                    slot.dropButton.onClick.AddListener(delegate { EventRelay.OnItemDropped(controller, item); });

                    
                    switch (item.itemTypeData)
                    {
                        case Consumable consumable:
                            
                            break;
                        case Resource resource:

                            break;
                        case Weapon weapon:
                            
                            slot.equipButton.onClick.AddListener(delegate { EventRelay.OnItemEquipped(controller, item); });
                            
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
            if (SelectionManager.CurrentlySelectedObject == null || SelectionManager.CurrentlySelectedObject as AiController == null) return;

            var controller = SelectionManager.CurrentlySelectedObject as AiController;

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
                    
                    slot.unEquipButton.onClick.AddListener(delegate { EventRelay.OnItemUnEquipped(controller,item); });
                    slot.dropButton.onClick.AddListener(delegate { EventRelay.OnItemDropped(controller,item); });
                    
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
        #endregion
        */
        
    }
}
