using UnityEngine;

namespace ProjectColoni
{
    public abstract class SmartAction : ScriptableObject
    {
        public abstract void Act(AiController aiController, SmartObject smartObject);
        /// <summary>
        /// Adds all the required contextual right click actions for object.
        /// </summary>
        public abstract void Initialize(SmartObject smartObject);
    }
}
