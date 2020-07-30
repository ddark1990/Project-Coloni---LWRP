using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ProjectColoni
{
    [RequireComponent(typeof(AiStats))]
    [RequireComponent(typeof(AiSensors))]
    public class AiBase : Selectable
    {
        [HideInInspector] public AiStats aiStats;
        [HideInInspector] public AiSensors sensors;
        
        [HideInInspector] public bool enableGizmos;

        
        protected void OnStartInitializeComponents()
        {
            aiStats = GetComponent<AiStats>();
            sensors = GetComponent<AiSensors>();

        }

        #region Gizmos
        
        private void OnDrawGizmos()
        {
            if (!enableGizmos) return;
            
            /*foreach (var target in sensors.overlapNonAllocResults)
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.magenta);
            }*/
        }
        #endregion
    }
}

