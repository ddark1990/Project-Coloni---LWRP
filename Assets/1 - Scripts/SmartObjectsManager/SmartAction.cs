using UnityEngine;

namespace ProjectColoni
{
    public abstract class SmartAction : ScriptableObject
    {
        [Tooltip("Add wait time before action plays. Can be 0. (MANUAL)")] public float animationLengthModifier;
        
        /// <summary>
        /// Agent starts the action once he is in range of stopping distance.
        /// Stopping distance is defined in the SmartObject component. Can be set from Initialize method as well.
        /// </summary>
        /// <param name="aiController"></param>
        /// <param name="smartObject"></param>
        public abstract void Act(AiController aiController, SmartObject smartObject);
        /// <summary>
        /// Adds all the required contextual right click actions for object.
        /// </summary>
        public abstract void Initialize(SmartObject smartObject);
    }
}
