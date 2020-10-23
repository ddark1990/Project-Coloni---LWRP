using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/NewActions/PickUpItem")]
    public class SmartAction_PickUpItem : SmartAction
    {
        public override void Act(AiController aiController, SmartObject smartObject)
        {
            PickUp(aiController, smartObject);
        }

        public override void Initialize(SmartObject smartObject)
        {
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["PickUp"], OnCliCkPickUp);
        }

        private void OnCliCkPickUp(AiController aiController, SmartObject smartObject)
        {
            smartObject.activeAction = this;
            smartObject.animationTrigger = "PickUp"; //might need a ref for diff types of inspect animations later on
            
            //smartObject.actionLength = aiController.GetRuntimeAnimationClipInfo(smartObject.animationTrigger).length; //get length of animation to be played
            smartObject.actionLength = aiController.GetRuntimeAnimationClipInfo(smartObject.animationTrigger).length + animationLengthModifier; 
            //aiController._animationWaitTime = animationLengthModifier;
            
            aiController.StartAction(smartObject, false); //commence the action based on given data
        }

        private void PickUp(AiController aiController, SmartObject smartObject)
        {
            //event
            EventRelay.OnItemPickedUp(smartObject as Item);
        }
    }
}
