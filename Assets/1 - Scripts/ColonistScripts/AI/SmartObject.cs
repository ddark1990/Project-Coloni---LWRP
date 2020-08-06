using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public class SmartObject : Selectable
    {
        [Header("SmartObject")]
        public bool beingUsed;
        public AiController usedBy;
        public float stoppingDistance = 0.2f;
    

    }
}
