using UnityEngine;

namespace ProjectColoni
{
    public abstract class SmartAction : ScriptableObject
    {
        [Tooltip("Wait time to activate the logic based on the animation. Can be 0. (MANUAL)")] public float animationLogicCounter;
        public abstract void Act(AiController aiController, SmartObject smartObject);
        /// <summary>
        /// Adds all the required contextual right click actions for object.
        /// </summary>
        public abstract void Initialize(SmartObject smartObject);
    }
}
