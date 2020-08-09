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
    }
}
