using System;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class SmartObject : Selectable
    {
        [Header("SmartObject")]
        public SmartActions smartActions;

        public bool beingUsed;
        public AiController usedBy;
        public float actionLength = 1;
        [Header("Settings")]
        public float stoppingDistance = 0.2f;
        [Header("Debug")]
        public string animationTrigger;

        public SmartAction activeAction;
        public Dictionary<Sprite, UnityAction<AiController, SmartObject>> smartActionDictionary; //dont rly need name cuz can get name from the actual action method

        private void OnEnable()
        {
            if (smartActionDictionary == null) smartActionDictionary = new Dictionary<Sprite, UnityAction<AiController, SmartObject>>();
        }

        public void AddSmartActionToCollection(Sprite actionSprite, UnityAction<AiController, SmartObject> unityAction)
        {
            smartActionDictionary.Add(actionSprite, unityAction);
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                OutlineHighlight();
            }

            if (usedBy != null && usedBy.actionInProgress)
            {
                
            }
        }

        public void SetSmartObjectData(AiController aiController)
        {
            usedBy = aiController;
            beingUsed = true;
        }

        public void ResetSmartObject()
        {
            usedBy = null;
            beingUsed = false;
            activeAction = null;
            animationTrigger = string.Empty;
            actionLength = 0;
        }

        public bool debugGizmosEnabled;
        
        private void OnDrawGizmos()
        {
            if (debugGizmosEnabled)
            {
                Gizmos.DrawWireSphere(transform.position, stoppingDistance);
            }
        }
    }
}
