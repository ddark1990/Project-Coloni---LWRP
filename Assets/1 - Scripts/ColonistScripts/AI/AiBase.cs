using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectColoni
{
    [RequireComponent(typeof(AiStats))]
    [RequireComponent(typeof(AiSensors))]
    public class AiBase : MonoBehaviour
    {
        [HideInInspector] public AiStats stats;
        [HideInInspector] public AiSensors sensors;
        
        [HideInInspector] public bool enableSensors;
        /*[HideInInspector] */public bool enableGizmos;
        
        private void Awake()
        {
            stats = GetComponent<AiStats>();
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

