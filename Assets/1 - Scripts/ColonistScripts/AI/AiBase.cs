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
        [Header("AiData")]
        [Tooltip("If null, will generate random values for the base data.")] 
        [SerializeField] private BaseScriptableData baseData; //for manual creation
        private BaseObjectData _baseObjectInfo;
        
        //
        [HideInInspector] public AiStats aiStats;
        [HideInInspector] public AiSensors sensors;
        
        [HideInInspector] public bool enableSensors;
        [HideInInspector] public bool enableGizmos;

        
        protected void OnStartInitializeComponents()
        {
            aiStats = GetComponent<AiStats>();
            sensors = GetComponent<AiSensors>();

        }
        
        protected void InitializeBaseObjectData() //initialize base data from a scriptable object if one is available 
        {
            _baseObjectInfo = baseData != null ? new BaseObjectData(baseData.name, baseData.description, baseData.sprite) 
                : new BaseObjectData("", "", null);
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

