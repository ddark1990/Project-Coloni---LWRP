using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/NewActions/PracticeShooting")]
    public class SmartAction_PracticeShooting : SmartAction
    {
        public override void Act(AiController aiController, SmartObject smartObject)
        {
            
        }

        public override void Initialize(SmartObject smartObject)
        {
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Shooting"], OnCliCkPracticeShooting);
        }
        
        private void OnCliCkPracticeShooting(AiController aiController, SmartObject smartObject)
        {
            smartObject.activeAction = this;
            smartObject.animationTrigger = "Shoot"; //might need a ref for diff types of inspect animations later on
            
            smartObject.actionLength = aiController.GetRuntimeAnimationClipInfo(smartObject.animationTrigger).length; //get length of animation to be played
            aiController._animationWaitTime = animationLengthModifier;
            
            aiController.StartAction(smartObject, true); //commence the action based on given data
        }
    }
}
