using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ItemData)", menuName = "ProjectColoni/Objects/Create Item/New Tool", order = 5)]
    public class Tool : ItemType
    {
        public enum ToolType
        {
            Axe,
            PickAxe
            
        }

        public ToolType toolType;
    }
}
