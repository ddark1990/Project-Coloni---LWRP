using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    [RequireComponent(typeof(AiStats))]
    [RequireComponent(typeof(AiSensors))]
    [RequireComponent(typeof(AiStatus))]
    public class AiBase : SmartObject
    {
        public enum AiType
        {
            Unity, AStar
        }

        [HideInInspector] public AiStats aiStats;
        [HideInInspector] public AiStateController aiStateController;
        [HideInInspector] public AiSensors sensors;
        [HideInInspector] public AiStatus status;
        [HideInInspector] public Ai_Inventory inventory;
        [HideInInspector] public Ai_Equipment equipment;
        [HideInInspector] public NavMeshAgent navMeshAgent; //unity pathfinding
        [HideInInspector] public AIPath aiPath; //Astar pathfinding
        [HideInInspector] public Animator animator;
        [HideInInspector] public Selectable selectable;
        [HideInInspector] public Camera cam;
        [HideInInspector] public LineRenderer destinationLineRenderer;
        [HideInInspector] public Rigidbody rigidBody;

        [HideInInspector] public bool enableGizmos;
        
        public AiType aiType;

        
        protected void OnStartInitializeComponents(AiController aiController)
        {
            aiStats = GetComponent<AiStats>();
            sensors = GetComponent<AiSensors>();
            status = GetComponent<AiStatus>();
            inventory = GetComponent<Ai_Inventory>();
            equipment = GetComponent<Ai_Equipment>();
            rigidBody = GetComponent<Rigidbody>();
            
            switch (aiType)
            {
                case AiType.Unity:
                    navMeshAgent = GetComponent<NavMeshAgent>();

                    break;
                case AiType.AStar:
                    aiPath = GetComponent<AIPath>();
                    
                    break;
            }
            
            aiStateController = new AiStateController(aiController);
            
            animator = GetComponent<Animator>();
            selectable = GetComponent<Selectable>();
            destinationLineRenderer = GetComponent<LineRenderer>();
            cam = SelectionManager.Instance.cam;

            animator.applyRootMotion = true;
        }

        private void Update()
        {
            //OutlineHighlight();
        }

        protected void DrawLineRendererPaths(NavMeshAgent agent, LineRenderer lineRenderer) //navigation path render line for in game *currently allocates 88 bytes per line drawn
        {
            if (agent.remainingDistance <= 0.1f || !selectable.selected) //should only be visible when that unit is selected
            {
                lineRenderer.enabled = false;
                return;
            }

            var agentPath = agent.path;
            
            lineRenderer.positionCount = agentPath.corners.Length;
            lineRenderer.SetPositions(agentPath.corners);
            lineRenderer.enabled = true;
        }
        
        #region Gizmos
        
        private void OnDrawGizmos()
        {
            if (!enableGizmos) return;
            
            /*foreach (var target in sensors.overlapNonAllocResults)
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.magenta);
            }*/
        }
        #endregion
    }
}

