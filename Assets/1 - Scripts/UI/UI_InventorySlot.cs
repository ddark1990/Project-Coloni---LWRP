using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_InventorySlot : Slot
    {
        public bool active;
        
        public Image itemIcon;
        public Text itemName;
        public Text itemDescription;
        public Text itemCount;
        public Text itemWeight;

        public Button useButton;
        public Button dropButton;
        public Button equipButton;
       
    }
}
