using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class Ai_CombatController
    {
        public enum CombatMode //by default melee is always used first
        {
            Melee,
            Ranged 
        }
        
        public bool combatModeMelee;
        public CombatMode combatMode;

        private AiController _controller;

        public Ai_CombatController(AiController controller)
        {
            _controller = controller;
            
            combatModeMelee = true;
            combatMode = CombatMode.Melee;
        }

        /// <summary>
        /// Toggle the mode which the Ai will combat in, Melee (fists or weapon) or Ranged (if has one equipped)
        /// </summary>
        public void ToggleCombatMode()
        {
            if (!_controller.equipment.IsEquipped(EquipmentSlot.EquipmentType.RangedWep))
            {
                Debug.Log("No Ranged Weapon Equipped!");
                return;
            }
            
            combatModeMelee = !combatModeMelee;
            
            combatMode = combatModeMelee ? CombatMode.Melee: CombatMode.Ranged;
            
            EventRelay.OnCombatModeToggled(_controller);
        }
        
        /// <summary>
        /// Toggles the the ability for the player to control the colonist movement directly with right clicks
        /// and puts the colonist in a drafted state where he can target non friendlies.
        /// </summary>
        public void ToggleDraftState()
        {
            _controller.stateController.Drafted = !_controller.stateController.Drafted;
            _controller.enablePlayerControl = _controller.stateController.Drafted;

            EventRelay.OnToggleDrafted(_controller);
        }

        public void Attack(SmartObject smartObject)
        {
            switch (smartObject)
            {
                case AiController aiController:
                    aiController.aiStats.TakeDamage(10);
                    
                    if(aiController.aiStats.stats.Health <= 0)
                        _controller.ResetAction(smartObject);
                    
                    Debug.Log("Attacking " + aiController + " | Health Left: " + aiController.aiStats.stats.Health);
                    break;
                case PlaceableObject placeableObject:
                    
                    break;
            }
        }
    }
}
