using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Create Item/Item Type/Consumable", order = 0)]
public class Consumable : ItemType
{
    [Serializable]
    public struct ConsumableSettings
    {
        public bool cookable;
        public bool raw;
        public float giveHealthAmount;
        public float giveCaloriesAmount;
        public float giveHydrationAmount;
    }

    public ConsumableSettings consumableSettings;
}