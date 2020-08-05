using System;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ItemData)", menuName = "ProjectColoni/Objects/Create Item/New Resource", order = 4)]
    public class Resource : ItemType
    {
        [Serializable]
        public struct ResourceSettings
        {
            public float health;
            public float food;
        }

        [Tooltip("How much & what you will gain from consuming this item.")] public ResourceSettings resourceSettings;
    }
}
