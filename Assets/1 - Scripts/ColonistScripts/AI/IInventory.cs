using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public interface IInventory
    {
        void OnItemPickUp(Item item);
        void OnItemDrop(Item item);
        void OnItemEquip(Item item);
        void OnItemUnEquip(Item item);
    }
}
