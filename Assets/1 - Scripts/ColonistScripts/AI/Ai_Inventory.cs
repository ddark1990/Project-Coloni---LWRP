using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class Ai_Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryHolder;
        
        public Dictionary<string, Item> holdingItems = new Dictionary<string, Item>();
        public float currentWeight; //default 15 lbs
        public float weightLimit = 15; //default 15 lbs
        
        public void AddItemToInventory(Item item)
        {
            if (holdingItems.ContainsKey(item.baseObjectInfo.Id) || 
                (currentWeight + item.itemData.itemData.itemWeight) > weightLimit) return;
            
            holdingItems.Add(item.baseObjectInfo.Id, item);
            currentWeight += item.itemData.itemData.itemWeight;
            
            SetItemTransform(item);
            UI_SelectionController.Instance.inventoryPanelController.UpdateInventoryUi(item);
        }

        private void SetItemTransform(Item item)
        {
            var itemTransform = item.transform;
            
            itemTransform.SetParent(inventoryHolder.transform);
            itemTransform.localPosition = new Vector3(0,0,0);
            
            item.gameObject.SetActive(false);

        }
    }
}
