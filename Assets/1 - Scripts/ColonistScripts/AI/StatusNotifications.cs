using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class StatusNotifications : MonoBehaviour
    {
        //public HashSet<StatusModifier> hashSet;
    }

    [CreateAssetMenu(fileName = "StatusModifier", menuName = "ProjectColoni/Objects/AI/StatusNotifications/New StatusModifier", order = 6)]
    public class StatusModifier : ScriptableObject
    {
        
    }
    
    [CreateAssetMenu(fileName = "StatusNotification", menuName = "ProjectColoni/Objects/AI/StatusNotifications/New StatusNotification", order = 6)]
    public class StatusNotification : ScriptableObject
    {
        
    }
}
