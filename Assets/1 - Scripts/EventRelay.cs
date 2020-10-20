using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public static class EventRelay
    {
        public static Action<Selectable> ObjectSelected { get; set; }
        public static Action ObjectDeSelected { get; set; }

        public static Action<Item> OnItemPickedUp { get; set; }
        public static Action<Item> OnItemDropped { get; set; }
        public static Action<Item> OnItemEquipped { get; set; }
        public static Action<Item> OnItemUnEquipped { get; set; }
        public static Action<AiController> OnToggleDrafted { get; set; }
        public static Action<AiController> OnCombatModeToggled { get; set; }
        
    }
}
