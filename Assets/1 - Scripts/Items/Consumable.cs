using System;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ItemData)", menuName = "ProjectColoni/Objects/Create Item/New Consumable", order = 4)]
    public class Consumable : ItemType
    {
        [Serializable]
        public struct ConsumableSettings
        {
            public float health;
            public float food;
        }

        [Tooltip("How much & what you will gain from consuming this item.")] public ConsumableSettings consumableSettings;

        public void Eat()
        {
            Debug.Log("Eating " + this);
        }
    }
}