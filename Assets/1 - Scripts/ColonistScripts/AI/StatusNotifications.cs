using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "StatusNotification", menuName = "ProjectColoni/Objects/AI/New StatusNotification", order = 6)]
    public class StatusNotification : ScriptableObject
    {
        public NotificationInfo notificationInfo;
        
        [Serializable]
        public struct NotificationInfo
        {
            public string statusName;
            [TextArea] public string statusDescription;
            public Sprite statusSprite;
            public Color statusColor;
        }
    }
}
