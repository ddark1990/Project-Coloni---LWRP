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

        private void OnClickAttack(AiController aiController, SmartObject smartObject)
        {
            _activeWeapon = aiController.equipment.GetActiveWeapon();
            
            smartObject.activeAction = this;
            //might need a ref for diff types of inspect animations later on
            smartObject.animationTrigger = _activeWeapon.animationAttackTrigger;
            smartObject.stoppingDistance = _activeWeapon.attackRange;
            //set length of action, in this case infinity until target is dead or action canceled 
            smartObject.actionLength = _activeWeapon.attackSpeed; 
            //allows a wait time between logic
            //aiController._animationWaitTime = _activeWeapon.attackSpeed;
            
            //commence the action based on given data
            aiController.StartAction(smartObject, true);
            //EventRelay.OnAttackInitiated(aiController);
        }

        private Weapon _activeWeapon;
        
        private void Attack(AiController aiController, SmartObject smartObject)
        {
            aiController.combatController.Attack(smartObject);
        }
    }
}
