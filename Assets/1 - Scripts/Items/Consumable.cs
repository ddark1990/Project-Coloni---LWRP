using System;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "ConsumableItem", menuName = "ProjectColoni/Objects/Create Item/New Consumable", order = 4)]
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
}