using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/NewActions/Action_Inspect")]
    public class SmartAction_Inspect : SmartAction
    {
        public override void Act(AiController aiController, SmartObject smartObject)
        {
            Inspect(aiController, smartObject);
        }

        public override void Initialize(SmartObject smartObject)
        {
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Inspect"], OnClickInspect);
        }

        private void OnClickInspect(AiController aiController, SmartObject smartObject)
        {
            smartObject.activeAction = this;
            smartObject.animationTrigger = "Inspect"; //might need a ref for diff types of inspect animations later on
            
            smartObject.actionLength = aiController.GetRuntimeAnimationClipInfo(smartObject.animationTrigger).length; //get length of animation to be played

            aiController.StartAction(smartObject); //commence the action based on given data
        }

        private void Inspect(AiController aiController, SmartObject smartObject) //action logic, ai is the activator that is currently selected, smartObject is the object activated from a right click action (SmartAction)
        {
            Debug.Log("Inspecting");
        }
    }
}
