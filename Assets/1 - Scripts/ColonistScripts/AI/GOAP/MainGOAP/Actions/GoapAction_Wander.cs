using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ProjectColoni
{
    public class GoapAction_Wander : GoapAction
    {
        public float waitMinTime = 1f;
        public float waitMaxTime = 1f;
        public float wanderRadius = 5f;

        public bool showWanderRadius;
    
        private float _tempWaitTime;
        private float _elapsedTime;
        private bool _completed;

        public GoapAction_Wander()
        {
            /*
            addPrecondition("hasTarget", false);
            addPrecondition("pickUpAvailable", false);
            addPrecondition("inCombat", false);
            */
            addEffect("wander", true);
        }

        public override void reset()
        {
            _tempWaitTime = Random.Range(waitMinTime, waitMaxTime);
            _elapsedTime = 0f;
            _completed = false;
        }

        public override bool isDone()
        {
            return _completed;
        }

        public override bool requiresInRange()
        {
            return false;
        }

        private Vector3 tempVector;
        public override bool checkProceduralPrecondition(AiController controller)
        {
            tempVector = StaticUtility.GetRandomRadialPos(transform, wanderRadius);
            return true;
        }

        public override bool perform(AiController controller)
        {
            if (_elapsedTime <= 0 && tempVector != transform.position) controller.aiPath.destination = tempVector;

            if (!(controller.aiPath.remainingDistance <= controller.aiPath.endReachedDistance)) return true;
            
            //Debug.Log("Started waiting.");
            _elapsedTime += Time.deltaTime;

            if (!(_elapsedTime >= _tempWaitTime)) return true;

            //Debug.Log("Finished waiting.");
            _completed = true;

            return true;
        }

        private void OnDrawGizmos()
        {
            if (!showWanderRadius) return;
            {
                var color = Color.Lerp(new Color(1f, 0.61f, 0.27f), new Color(0.23f, 0.89f, 1f), Mathf.PingPong(Time.time, 1));

                Gizmos.color = color;
                Gizmos.DrawWireSphere(transform.position, wanderRadius);
            }
        }
    }
}
