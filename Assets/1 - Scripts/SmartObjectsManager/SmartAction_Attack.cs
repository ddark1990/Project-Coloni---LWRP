using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(menuName = "ProjectColoni/SmartActions/NewActions/Attack")]
    public class SmartAction_Attack : SmartAction
    {
        public override void Act(AiController aiController, SmartObject smartObject)
        {
            Attack(aiController, smartObject);
        }

        public override void Initialize(SmartObject smartObject) 
        {
            smartObject.AddSmartActionToCollection(
                GameManager.Instance.globalSpriteContainer.spriteCollection["Shooting"], OnClickAttack);
        }

        //should put in draft mode if not already
        private void OnClickAttack(AiController aiController, SmartObject smartObject)
        {
            //cache, maybe
            var activeWeapon = aiController.equipment.activeWeapon;
            
            smartObject.activeAction = this;
            //might need a ref for diff types of inspect animations later on
            smartObject.animationTrigger = activeWeapon.animationAttackTrigger;
            smartObject.stoppingDistance = activeWeapon.attackRange;
            //set length of action, in this case infinity until target is dead or action canceled 
            smartObject.actionLength = activeWeapon.attackSpeed; 
            //allows a wait time between logic
            //aiController._animationWaitTime = _activeWeapon.attackSpeed;
            
            //commence the action based on given data
            aiController.StartAction(smartObject, true);
            //EventRelay.OnAttackInitiated(aiController);
        }
        
        private void Attack(AiController aiController, SmartObject smartObject)
        {
            aiController.combatController.Attack(smartObject);
        }
    }
}
