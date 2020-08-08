using System;
using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public class SmartObject : Selectable
    {
        [Header("SmartObject")]
        public bool beingUsed;
        public AiController usedBy;
        public float actionLength = 1;
        public float stoppingDistance = 0.2f;
        public string animationTrigger;

        public void SetSmartObjectData(AiController aiController)
        {
            usedBy = aiController;
            beingUsed = true;
        }

        public void ResetSmartObject()
        {
            usedBy = null;
            beingUsed = false;
        }
        
        //general
        protected void Inspect(AiController aiController) //should be able to inspect pretty much anything selectable 
        {
            animationTrigger = "Inspect";

            actionLength = aiController.GetRuntimeAnimationClipInfo(animationTrigger).length;
            
            aiController.StartAction(this);
            
        }
        
        //items
        protected void PickUp(AiController aiController) //pick up mostly items
        {
            animationTrigger = "PickUp";
            actionLength = 1;
            
            aiController.StartAction(this);

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
