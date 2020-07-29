using System;
using UnityEngine;

namespace ProjectColoni
{
    public class ItemType : ScriptableObject
    {
        [Serializable]
        public struct ItemData
        {
            public string itemName;
            [TextArea] public string itemDescription;
            public Sprite itemSprite;
            public int stackLimit;
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
