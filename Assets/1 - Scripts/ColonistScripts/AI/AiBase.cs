using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectColoni
{
    [RequireComponent(typeof(AiVitals))]
    [RequireComponent(typeof(AiSensors))]
    public class AiBase : MonoBehaviour
    {
        [HideInInspector] public AiVitals vitals;
        [HideInInspector] public AiSensors sensors;
        
        [HideInInspector] public bool enableSensors;
        /*[HideInInspector] */public bool enableGizmos;
        
        private void Awake()
        {
            vitals = GetComponent<AiVitals>();
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

