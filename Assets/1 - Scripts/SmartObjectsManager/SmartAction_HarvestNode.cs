using System;
using UnityEngine;
using static ProjectColoni.Resource;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/NewActions/Action_HarvestNode")]
    public class SmartAction_HarvestNode : SmartAction
    {
        public override void Act(AiController aiController, SmartObject smartObject)
        {
            Harvest(aiController, smartObject);
        }

        public override void Initialize(SmartObject smartObject)
        {
            smartObject.AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Harvest"], OnClickHarvest);
        }

        private void OnClickHarvest(AiController aiController, SmartObject smartObject)
        {
            smartObject.activeAction = this;

            switch (((ResourceNode) ((Node) smartObject).nodeData).resourceSettings.resourceType)
            {
                case ResourceNode.ResourceSettings.ResourceType.Bush:
                    smartObject.animationTrigger = "GatherBush";
                    break;
                case ResourceNode.ResourceSettings.ResourceType.Stone:
                    smartObject.animationTrigger = "GatherStone";
                    break;
                case ResourceNode.ResourceSettings.ResourceType.Wood:
                    smartObject.animationTrigger = "GatherWood";
                    break;
                case ResourceNode.ResourceSettings.ResourceType.Variety:
                    break;
            }
            
            smartObject.actionLength = Mathf.Infinity; 

            aiController.StartAction(smartObject, true);
        }

        private void Harvest(AiController aiController, SmartObject smartObject)
        {
            Debug.Log("Harvesting");

        }
    }
}
