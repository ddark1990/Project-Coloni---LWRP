using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/SmartActions", order = 0, fileName = "SmartActions_")]
    public class SmartActions : ScriptableObject
    {
        [Tooltip("Right click actions.")] public SmartAction[] actions;

        public void InitializeSmartActions(SmartObject smartObject)
        {
            if (actions.Length == 0) return;
            
            foreach (var action in actions)
            {
                action.Initialize(smartObject);
            }
        }
    }
}
