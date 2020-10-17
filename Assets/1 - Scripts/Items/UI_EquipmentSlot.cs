using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_EquipmentSlot : Slot
    {
        public EquipmentSlot slotReference;
        [Header("Cache")]
        public EquipmentSlot.EquipmentType equipmentType; //directs where the gear is equiped
        public Image icon;
        public Button unEquipButton;
        public Button dropButton;
    }
}
