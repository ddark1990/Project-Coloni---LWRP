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
    }
}
