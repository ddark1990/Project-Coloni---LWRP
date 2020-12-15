using System;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ItemData)", menuName = "ProjectColoni/Objects/Create Item/New Resource", order = 4)]
    public class Resource : ItemType
    {
        public enum ResourceType
        {
            Stone, Wood
        }
            
        public ResourceType resourceType;
        public StackLevel[] stackLevels;

        [Serializable]
        public struct StackLevel
        {
            public string name;
            public Mesh skin;
        }
    }
}
