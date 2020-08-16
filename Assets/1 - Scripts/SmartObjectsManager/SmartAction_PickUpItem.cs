﻿using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/NewActions/PickUpItem")]
    public class SmartAction_PickUpItem : SmartAction
    {
        public override void Act(AiController aiController, SmartObject smartObject)
        {
            PickUp();
        }

        public override void Initialize(SmartObject smartObject)
        {
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["PickUp"], OnCliCkPickUp);
        }

        private void OnCliCkPickUp(AiController aiController, SmartObject smartObject)
        {
            smartObject.activeAction = this;
            
            smartObject.animationTrigger = "PickUp"; //might need a ref for diff types of inspect animations later on
            smartObject.actionLength = aiController.GetRuntimeAnimationClipInfo(smartObject.animationTrigger).length; //get length of animation to be played

            aiController.StartAction(smartObject); //commence the action based on given data
        }

        private void PickUp()
        {
            
        }
    }
}
