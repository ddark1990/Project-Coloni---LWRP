using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AI_AnimalGOAPAgent : GoapAgent, IGoap
    {
        public bool goalDebug;
    
        private Vector3 previousDestination;
        private HashSet<KeyValuePair<string, object>> worldData;
        private HashSet<KeyValuePair<string, object>> goalData;
    
        private void OnEnable()
        {
            aiController = GetComponent<AiController>(); //get main AI controller
            InitializeAgent();
        }
    
        /**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
        public HashSet<KeyValuePair<string,object>> getWorldState()
        {
            worldData = new HashSet<KeyValuePair<string, object>>();

            worldData.Add(new KeyValuePair<string, object>("isHungry", aiController.aiStats.IsHungry));
            worldData.Add(new KeyValuePair<string, object>("forcedAction", aiController.performingForcedAction));
        
            return worldData;
        }

        public HashSet<KeyValuePair<string, object>> createGoalState()
        {
            goalData = new HashSet<KeyValuePair<string, object>>( );

            goalData.Add(new KeyValuePair<string, object>("wander", true));
        
            return goalData;
        }
    
        public void planFailed(HashSet<KeyValuePair<string,object>> failedGoal)
        {
            // Not handling this here since we are making sure our goals will always succeed.
            // But normally you want to make sure the world state has changed before running
            // the same goal again, or else it will just fail.
            if(goalDebug) Debug.Log("failedGoal");
        }

        public void planFound(HashSet<KeyValuePair<string,object>> goal, Queue<GoapAction> actions)
        {
            // Yay we found a plan for our goal
            if(goalDebug) Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
        }

        public void actionsFinished()
        {
            // Everything is done, we completed our actions for this gool. Hooray!
            if(goalDebug) Debug.Log("<color=blue>Actions completed</color>");
        }

        public void planAborted(GoapAction aborter)
        {
            // An action bailed out of the plan. State has been reset to plan again.
            // Take note of what happened and make sure if you run the same goal again
            // that it can succeed.
            if(goalDebug) Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
        }

        public bool moveAgent(GoapAction nextAction)
        {
            //if we don't need to move anywhere
            if (previousDestination == nextAction.target.transform.position)
            {
                nextAction.setInRange(true);
                return true;
            }
         
            if (!aiController.aiStats.IsDead)
            {
                aiController.aiPath.destination = nextAction.target.transform.position;
            }

            if (aiController.aiPath.hasPath && !aiController.aiPath.pathPending && aiController.aiPath.remainingDistance <= aiController.aiPath.endReachedDistance)
            {
                nextAction.setInRange(true);
                previousDestination = nextAction.target.transform.position;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}