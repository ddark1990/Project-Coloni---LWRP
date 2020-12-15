using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace ProjectColoni
{
    public class Ai_Equipment : MonoBehaviour
    {
        [Header("Active Equipment")] 
        public Weapon activeWeapon;
        
        [Header("Slots")]
        public EquipmentSlot[] equipmentSlots;
        
        [HideInInspector] public WeaponHolderRelay weaponHolderRelay;
        
        private AiController _controller;


        private void OnEnable()
        {
            EventRelay.OnItemEquipped += EquipItem;
            EventRelay.OnItemUnEquipped += UnEquipItem;

            EventRelay.OnToggleDrafted += ActivateWeaponModel;
            EventRelay.OnCombatModeToggled += ActivateWeaponModel;
        }

        private void EquipItem(AiController controller, Item item)
        {
            _controller = controller;
            
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (controller == null) break;
                
                var slot = equipmentSlots[i];
                
                //make sure equipment types match between slot and item
                if (slot.equipmentType.Equals(item.itemTypeData.itemData.equipmentType)) 
                {
                    if (slot.equippedItem != null) //if another item equipped, un equip old item first
                    {
                        controller.equipment.UnEquipItem(controller, slot.equippedItem);
                    }
                    
                    slot.equippedItem = item;
                    item.equipped = true;

                    //creates item model from the items type model reference,
                    //if the item has a modelReference, stays null if item is not
                    slot.modelReference = CreateWeaponModel(item); 
                    
                    /*
                    UI_Controller.Instance.inventoryPanelController.UpdateEquipmentUi(item, slot, true);
                    UI_Controller.Instance.inventoryPanelController.UpdateInventoryUi(controller, item, false);
                */
                }
            }
        }

        private void UnEquipItem(AiController controller, Item item)
        {
            foreach (var slot in equipmentSlots)
            {
                if (slot.equipmentType.Equals(item.itemTypeData.itemData.equipmentType))
                {
                    item.equipped = false;
                    /*
                    UI_Controller.Instance.inventoryPanelController.UpdateEquipmentUi(item, slot, false);
                    UI_Controller.Instance.inventoryPanelController.UpdateInventoryUi(controller, item, true);
                    */
                    slot.equippedItem = null;
                    
                    ClearWeaponModel(slot); 
                }
            }
        }

        public bool IsEquipped(EquipmentSlot.EquipmentType type)
        {
            foreach (var slot in equipmentSlots)
            {
                if (slot.equipmentType == type && slot.equippedItem != null) return true;
            }

            return false;
        }

        public EquipmentSlot GetEquipmentSlot(EquipmentSlot.EquipmentType type)
        {
            foreach (var slot in equipmentSlots)
            {
                if (slot.equipmentType == type) return slot;
            }

            return null;
        }
        
        private WeaponModel CreateWeaponModel(Item item) 
        {
            if (!(item.itemTypeData is Weapon weapon)) return null;
            
            var controllerTransform = _controller.equipment.weaponHolderRelay.transform;
                        
            var model = Instantiate(weapon.modelReference, controllerTransform);
            var modelTransform = model.transform;
            
            modelTransform.localPosition = weapon.modelTransform.position;
            modelTransform.localRotation = Quaternion.Euler(weapon.modelTransform.rotation);
                        
            //change back to normal scale cuz root bone of the dummy colonist is at 0.01
            modelTransform.localScale = new Vector3(100,100,100); 
            
            //toggle the created model off
            ToggleWeaponModel(model, false); 
            
            _controller.equipment.weaponHolderRelay.weaponModels.Add(model);

            return model;
        }

        private EquipmentSlot _activeCombatEquipmentSlot;
        private WeaponModel _activeModelReference;

        private void ActivateWeaponModel(AiController controller) //event being fired twice issue
        {
            if (_activeModelReference != null)
            {
                StartCoroutine(Coroutine_ToggleWeaponModel(_activeCombatEquipmentSlot.modelReference, false, _shortWait));
                _activeModelReference = null;
            }
            
            switch (controller.combatController.combatMode)
            {
                case Ai_CombatController.CombatMode.Melee:
                    _activeCombatEquipmentSlot = controller.equipment.GetEquipmentSlot(EquipmentSlot.EquipmentType.MeleeWep);
                    break;
                case Ai_CombatController.CombatMode.Ranged:
                    _activeCombatEquipmentSlot = controller.equipment.GetEquipmentSlot(EquipmentSlot.EquipmentType.RangedWep);
                    break;
            }

            if (_activeCombatEquipmentSlot.modelReference != null && controller.stateController.Drafted && _activeModelReference == null)
            {
                StartCoroutine(Coroutine_ToggleWeaponModel(_activeCombatEquipmentSlot.modelReference, true, _shortWait));
                _activeModelReference = _activeCombatEquipmentSlot.modelReference;
            }
            
            activeWeapon = GetActiveWeapon(controller);
        }
        
        //animation related stuff, should move
        private readonly WaitForSeconds _shortWait = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _longWait = new WaitForSeconds(1f);
        
        private IEnumerator Coroutine_ToggleWeaponModel(WeaponModel model, bool active, WaitForSeconds waitForSeconds)
        {
            yield return waitForSeconds;
                
            model.gameObject.SetActive(active);
        }

        private void ToggleWeaponModel(WeaponModel model, bool active)
        {
            model.gameObject.SetActive(active);
        }
        
        private void ClearWeaponModel(EquipmentSlot slot)
        {
            Destroy(slot.modelReference.gameObject);
            //slot.modelReference = null;
        }

        public Weapon GetActiveWeapon(AiController controller)
        {
            if (!controller.stateController.Drafted)
            {
                Debug.Log("Not in drafted state.", this);
                return null;
            }
            
            switch (controller.combatController.combatMode)
            {
                case Ai_CombatController.CombatMode.Melee:
                    if (GetEquipmentSlot(EquipmentSlot.EquipmentType.MeleeWep).equippedItem != null)
                        return GetEquipmentSlot(EquipmentSlot.EquipmentType.MeleeWep).equippedItem
                            .itemTypeData as Weapon;
                    else
                        return null;
                case Ai_CombatController.CombatMode.Ranged:
                    return GetEquipmentSlot(EquipmentSlot.EquipmentType.RangedWep).equippedItem.itemTypeData as Weapon;
            }

            return null;
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
            MeleeWep,
            HarvestingTool
        }

        public string name;
        public EquipmentType equipmentType;
        public Item equippedItem;
        public WeaponModel modelReference;
    }
}
