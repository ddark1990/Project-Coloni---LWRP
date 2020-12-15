using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class Ai_Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryHolder;
        
        public Dictionary<string, Item> holdingItems = new Dictionary<string, Item>();
        public float currentWeight; 
        public float weightLimit = 15; //default 15 lbs

        private void OnEnable()
        {
            EventRelay.OnItemPickedUp += AddItemToInventory;
            EventRelay.OnItemDropped += DropInventoryItem;
        }

        private void AddItemToInventory(AiController controller, Item item)
        {
            if (holdingItems.ContainsKey(item.baseObjectInfo.Id) || 
                (currentWeight + item.itemTypeData.itemData.itemWeight) > weightLimit) return;
            
            holdingItems.Add(item.baseObjectInfo.Id, item);
            currentWeight += item.itemTypeData.itemData.itemWeight;
            
            SetItemTransform(item, inventoryHolder.transform, new Vector3(0,0,0), false);

            EventRelay.OnInventoryUpdated(controller);
        }
        
        private void DropInventoryItem(AiController controller, Item item)
        {
            if (!holdingItems.ContainsKey(item.baseObjectInfo.Id)) return;
            
            holdingItems.Remove(item.baseObjectInfo.Id);
            currentWeight -= item.itemTypeData.itemData.itemWeight;

            SetItemTransform(item, null, item.usedBy.transform.position + new Vector3(0,1.4f,0), true);
            item.GetComponent<Rigidbody>().AddForce(transform.forward + new Vector3(0,1,0) * 2, ForceMode.VelocityChange);
            
            //EventRelay.OnItemDropped(controller, item);
            EventRelay.OnInventoryUpdated(controller);
        }

        public void DropEquippedItem(Item item)
        {
            if (!holdingItems.ContainsKey(item.baseObjectInfo.Id)) return;

            holdingItems.Remove(item.baseObjectInfo.Id);
            currentWeight -= item.itemTypeData.itemData.itemWeight;

            SetItemTransform(item, null, item.usedBy.transform.position + new Vector3(0,1.4f,0), true);
            item.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,1), ForceMode.VelocityChange);
            
            //UI_Controller.Instance.inventoryPanelController.ClearEquipmentSlotData(item);
        }

        private void SetItemTransform(Item item, Transform setParent, Vector3 setPosition, bool active)
        {
            var itemTransform = item.transform;
            
            itemTransform.SetParent(setParent);
            itemTransform.localPosition = setPosition;
            
            item.gameObject.SetActive(active);

        }

    }
}
