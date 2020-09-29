using System;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    [RequireComponent(typeof(AiStats))]
    [RequireComponent(typeof(AiSensors))]
    [RequireComponent(typeof(AiStatus))]
    public class AiBase : SmartObject
    {
        [HideInInspector] public AiStats aiStats;
        [HideInInspector] public AiSensors sensors;
        [HideInInspector] public AiStatus status;
        [HideInInspector] public Ai_Inventory inventory;
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Selectable selectable;
        [HideInInspector] public Camera cam;
        [HideInInspector] public LineRenderer destinationLineRenderer;
        [HideInInspector] public Rigidbody rigidBody;

        [HideInInspector] public bool enableGizmos;

        
        protected void OnStartInitializeComponents()
        {
            aiStats = GetComponent<AiStats>();
            sensors = GetComponent<AiSensors>();
            status = GetComponent<AiStatus>();
            inventory = GetComponent<Ai_Inventory>();
            rigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            selectable = GetComponent<Selectable>();
            destinationLineRenderer = GetComponent<LineRenderer>();
            cam = SelectionManager.Instance.cam;

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

