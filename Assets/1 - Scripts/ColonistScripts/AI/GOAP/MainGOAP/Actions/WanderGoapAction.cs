using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ProjectColoni
{
    public class WanderGoapAction : GoapAction
    {
        public float waitMinTime = 1f;
        public float waitMaxTime = 1f;
        public float wanderRadius = 5f;

        public bool showWanderRadius;
    
        private float _tempWaitTime;
        private float _elapsedTime;
        private NavMeshPath _path;
        private bool _completed;

        public WanderGoapAction()
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

        public override bool checkProceduralPrecondition(AiController controller)
        {
            return true;
        }

        public override bool perform(AiController controller)
        {
            if (controller.aiStats.IsDead) return false;
        
            if (_elapsedTime <= 0 && !controller.navMeshAgent.hasPath)
            {
                //Debug.Log("Starting to wander.");
                _path = new NavMeshPath();

                NavMesh.CalculatePath(controller.navMeshAgent.transform.position, StaticUtility.GetRandomRadialPos(transform, wanderRadius), NavMesh.AllAreas, _path);
                //Debug.Log("Path Calculated for " + agent.name);

                controller.navMeshAgent.SetPath(_path);
                //Debug.Log("Path Set for " + agent.name);
            }

            if (!(controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)) return true;
            
            //Debug.Log("Started waiting.");
            _elapsedTime += Time.deltaTime;

            if (!(_elapsedTime >= _tempWaitTime)) return true;
            
            //Debug.Log("Finished waiting.");
            controller.navMeshAgent.ResetPath();
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
