using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ResourceNode)", menuName = "ProjectColoni/Objects/Create Node/New Resource Node", order = 2)]
    public class ResourceNode : NodeType
    {
        [Serializable]
        public struct ResourceSettings
        {
            public enum ResourceType
            {
                Stone, Wood, Bush, Variety
            }
            
            public ResourceType resourceType;
            public GameObject[] resources;
        }

        public ResourceSettings resourceSettings;
        [Range(0, 20)] public float gatherSpeed; //gather intervals

    }
}
