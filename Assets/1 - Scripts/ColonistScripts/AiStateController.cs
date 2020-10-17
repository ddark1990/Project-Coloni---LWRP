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

        private AiController _aiController; //in case for now

        public AiStateController(AiController aiController)
        {
            _aiController = aiController;
        }

        /// <summary>
        /// Toggles the the ability for the player to control the colonist directly with right clicks & puts the colonist in a drafted state where he can do combat.
        /// </summary>
        public void ToggleDraftState()
        {
            Drafted = !Drafted;
            _aiController.enablePlayerControl = Drafted;
        }
    }
}
