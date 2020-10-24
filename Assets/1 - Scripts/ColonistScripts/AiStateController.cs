using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiStateController
    {
        public bool Idle;
        public bool Drafted;
        public bool InCombat;

        private readonly AiController _aiController; //in case for now

        public AiStateController(AiController aiController)
        {
            _aiController = aiController;
        }

        public void DraftedToggled(AiController controller) 
        {
            if (controller.equipment.activeWeapon == null) return;
            
            if(AnimationController.TryGetAnimatorParam(  controller.animator, 
                controller.animationController.lastAnimatorCache, 
                controller.animationController.animatorParamCache, 
                controller.equipment.activeWeapon.animationDrawTrigger, 
                out controller.animationController.hash ) ) 
            {
                controller.animator.SetTrigger(controller.equipment.activeWeapon.animationDrawTrigger);
            }
        }
        public void CombatModeToggled(AiController controller)
        {
            if (controller.equipment.activeWeapon == null) return;

            if(AnimationController.TryGetAnimatorParam(  controller.animator, 
                controller.animationController.lastAnimatorCache, 
                controller.animationController.animatorParamCache, 
                controller.equipment.activeWeapon.animationDrawTrigger, 
                out controller.animationController.hash ) ) 
            {
                controller.animator.SetTrigger(controller.equipment.activeWeapon.animationDrawTrigger);
            }
        }
    }
}
