using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiStatus : MonoBehaviour
    {
        public List<StatusNotification> statusNotifications;
        
        
        public void AddNotification(StatusNotification notification)
        {
            if (!statusNotifications.Contains(notification)) statusNotifications.Add(notification);
        }

        public void RemoveNotification(StatusNotification notification)
        {
            if(statusNotifications.Contains(notification)) statusNotifications.Remove(notification);
        }
        
        /*public void AddModifier(StatusModifier modifier)
        {
            if (statusNotifications.Contains(modifier) && modifier.stackable)
            {
                //apply stack on modifier
            }
            else statusNotifications.Add(modifier);
        }

        public void AddModifiers(IEnumerable<StatusModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                if (statusNotifications.Contains(modifier) && modifier.stackable)
                {
                    //apply stack on modifier
                }
                else if (!statusNotifications.Contains(modifier)) AddModifier(modifier);
            }
        }
        
        public void RemoveModifier(StatusModifier modifier)
        {
            if(statusNotifications.Contains(modifier)) statusNotifications.Remove(modifier);
        }*/
    }
}
