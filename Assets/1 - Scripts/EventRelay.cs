using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public static class EventRelay
    {
        //selection
        public static Action<Selectable> OnObjectSelected { get; set; }
        public static Action<Selectable> OnObjectDeselected { get; set; }

        //inventory
        public static Action<AiController> OnInventoryUpdated { get; set; } 
        public static Action<AiController, Item> OnItemPickedUp { get; set; }
        public static Action<AiController, Item> OnItemDropped { get; set; }
        public static Action<AiController, Item> OnItemEquipped { get; set; }
        public static Action<AiController, Item> OnItemUnEquipped { get; set; }
        public static Action<int> OnItemCountChange { get; set; }
        
        //combat
        public static Action<AiController> OnToggleDrafted { get; set; }
        public static Action<AiController> OnCombatModeToggled { get; set; }
        public static Action<SmartObject> OnAttackInitiated { get; set; }
        public static Action<SmartObject> OnCombatModeInitiated { get; set; }

        
    }
}
