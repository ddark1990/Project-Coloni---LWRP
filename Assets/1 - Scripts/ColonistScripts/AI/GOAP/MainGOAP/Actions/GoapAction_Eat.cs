using UnityEngine;

namespace ProjectColoni
{
    public class GoapAction_Eat : GoapAction
    {
        public float extraWaitTime = 0.5f;
    
        private float _startTime;
        private float _elapsedTime;
        private float _tempWaitTime;

        private bool _completed;
        private Item _targetItem;

        public GoapAction_Eat()
        {
            addPrecondition("pickUpAvailable", true);
            addEffect("pickUpItem", true);
        }

        public override void reset()
        {
            _startTime = 0f;
            _completed = false;
        }

        public override bool isDone()
        {
            return _completed;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override bool checkProceduralPrecondition(AiController controller)
        {
            return true;
        }
    
        public override bool perform(AiController controller)
        {
            //Debug.Log("Started waiting.");
            _elapsedTime += Time.deltaTime;

            if (!(_elapsedTime >= _tempWaitTime)) return true;
            
            //Debug.Log("Finished waiting.");
            controller.navMeshAgent.ResetPath();
            _completed = true;

            return true;
        }
    }
}
