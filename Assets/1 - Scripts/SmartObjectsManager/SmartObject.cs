using System;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;
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
        public float stoppingDistance = 0.2f;
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

            if (usedBy != null && usedBy.inAction)
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
        
        //general
        protected void Inspect(AiController aiController) //should be able to inspect pretty much anything selectable 
        {
            animationTrigger = "Inspect";

            actionLength = aiController.GetRuntimeAnimationClipInfo(animationTrigger).length;
            
            //aiController.StartAction(this);
            
        }
        
        //items
        protected void PickUp(AiController aiController) //pick up mostly items
        {
            animationTrigger = "PickUp";
            actionLength = aiController.GetRuntimeAnimationClipInfo(animationTrigger).length;
            
            //aiController.StartAction(this);
            
            /*
            if(aiController.InRange(this))
                aiController.inventory.AddItemToInventory(this as Item);
                */

        }
        protected void Drop(AiController aiController)
        {
            
        }
        protected void Eat(AiController aiController)
        {
            
        }
        
        //resources
        protected void HaulAway(AiController aiController) //haul away items that are too heavy to carry inside inventory
        {
            
        }
        protected void Gather(AiController aiController) //gather only from resource nodes of some sort
        {
            switch (gameObject.tag)
            {
                case "Wood":
                    animationTrigger = "GatherWood";
                    break;
                case "Stone":
                    animationTrigger = "GatherStone";
                    break;
            }
            actionLength = 5;
            
            //aiController.StartAction(this);
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
