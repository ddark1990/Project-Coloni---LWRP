using System;
using UnityEngine;

namespace ProjectColoni
{
    public class ItemType : ScriptableObject
    {
        [Serializable]
        public struct ItemData
        {
            public int stackLimit;
            public Sprite inventorySprite;
            [Tooltip("Create modifiers by right clicking and going to ProjectColoni/Objects/AI/StatusNotifications/New StatusModifier")] 
            public StatusModifier[] statusModifiers;
            
        }
        /*
    [Serializable]
    public struct ItemSoundData
    {
        [Header("Interact Sounds")]
        public AudioClip activateItemSound;

        [Header("Item Pickup/Drop Sounds")]
        public AudioClip pickUpItemSound;
        public AudioClip throwItemSound;

        [Header("Item UI Sounds")]
        public AudioClip[] selectSounds;
        public AudioClip dragSound;
        public AudioClip dropSound;
    }
    */

        public ItemData itemData;
        //public ItemSoundData itemSoundData;
    }
}
