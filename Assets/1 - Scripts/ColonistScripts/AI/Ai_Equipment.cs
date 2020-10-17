using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectColoni
{
    public class Ai_Equipment : MonoBehaviour
    {
        public EquipmentSlot[] equipmentSlots;
        
        private AiController _controller;

        public void EquipItem(Item item)
        {
            _controller = SelectionManager.Instance.currentlySelectedObject as AiController;

            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                var slot = equipmentSlots[i];

                if (slot.equipmentType.Equals(item.itemTypeData.itemData.equipmentType))
                {
                    if (slot.equippedItem != null)
                    {
                        if (_controller != null) _controller.equipment.UnEquipItem(slot.equippedItem);
                    }
                    
                    slot.equippedItem = item;
                    item.equipped = true;
                    UI_SelectionController.Instance.inventoryPanelController.UpdateEquipmentUi(item, slot, true);
                    UI_SelectionController.Instance.inventoryPanelController.UpdateInventoryUi(item, false);
                }
            }
        }

        public void UnEquipItem(Item item)
        {
            foreach (var slot in equipmentSlots)
            {
                if (slot.equipmentType.Equals(item.itemTypeData.itemData.equipmentType))
                {
                    item.equipped = false;
                    UI_SelectionController.Instance.inventoryPanelController.UpdateEquipmentUi(item, slot, false);
                    UI_SelectionController.Instance.inventoryPanelController.UpdateInventoryUi(item, true);
                    slot.equippedItem = null;
                }
            }
        }

        public bool IsEquipped(EquipmentSlot.EquipmentType type)
        {
            return equipmentSlots.Any(slot => slot.equipmentType.Equals(type) && slot.equippedItem != null);
        }
    }

    [Serializable]
    public class EquipmentSlot
    {
        public enum EquipmentType
        {
            Head, 
            Chest, 
            Legs, 
            Feet, 
            Hands,
            ChestArmor,
            LegsArmor,
            Backpack,
            RangedWep,
            MeleeWep
        }

        public string name;
        public EquipmentType equipmentType;
        public Item equippedItem;
    }
}
