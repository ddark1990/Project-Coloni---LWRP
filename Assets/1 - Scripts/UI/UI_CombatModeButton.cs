using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_CombatModeButton : MonoBehaviour
    {
        //swap icons later on
        //bug: doozy turns on buttons visibility, disable doozy/ use reg animation 
        public Text combatModeText;
        public CanvasGroup canvasGroup;

        private string _combatButtonText;

        private void OnEnable()
        {
            EventRelay.OnCombatModeToggled += UpdateButtonText;
            EventRelay.OnToggleDrafted += UpdateButtonVisibility;
        }
        
        private void UpdateButtonVisibility(AiController controller)
        {
            canvasGroup.alpha = controller.stateController.Drafted ? 1 : 0;
            canvasGroup.interactable = controller.stateController.Drafted;
        }
        
        private void UpdateButtonText(AiController controller)
        {
            switch (controller.combatController.combatMode)
            {
                case Ai_CombatController.CombatMode.Ranged:
                    _combatButtonText = "R";
                    break;
                case Ai_CombatController.CombatMode.Melee:
                    _combatButtonText = "M";
                    break;
            }

            combatModeText.text = _combatButtonText;
        }
    }
}
