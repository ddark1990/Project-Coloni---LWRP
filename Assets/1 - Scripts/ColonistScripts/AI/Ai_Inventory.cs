using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class Ai_Inventory : MonoBehaviour
    {
        public Dictionary<string, Item> holdingItems = new Dictionary<string, Item>();

        public void AddItemToInventory(Item item)
        {
            if (holdingItems.ContainsKey(item.baseObjectInfo.Id)) return;
            
            holdingItems.Add(item.baseObjectInfo.Id, item);
            
            SetItemTransform(item);
        }

        private void SetItemTransform(Item item)
        {
            var itemTransform = item.transform;
            
            itemTransform.SetParent(transform);
            itemTransform.localPosition = new Vector3(0,0,0);
            
            item.gameObject.SetActive(false);

        }
    }
}
