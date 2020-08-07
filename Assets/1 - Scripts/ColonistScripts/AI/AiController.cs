using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public class AiController : AiBase
    {
        [Header("Ai")]
        public bool performingForcedAction;
        
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;
        private Selectable _selectable;
        private Camera _camera;
        private LineRenderer _destinationLineRenderer;


        private void Start()
        {
            GetComponents();
            
            OnStartInitializeComponents();
            InitializeSelectable();
        }
        
        private void Update()
        {
            DrawLineRendererPaths(navMeshAgent, _destinationLineRenderer);
            OutlineHighlight();
            
            /*
            if (Input.GetMouseButtonDown(1) && _selectable.selected)
            {
                //SetDestinationToMousePosition();
            }
        */
            UpdateAction(_tempSmartObject);
        }
        private void GetComponents()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            _selectable = GetComponent<Selectable>();
            _destinationLineRenderer = GetComponent<LineRenderer>();
            _camera = SelectionManager.Instance.cam;
        }
        
        /*private void SetDestinationToMousePosition()
        {
            if (_camera == null) return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;

            MoveAgent(hit.point);
            OnGroundClickSpawn(hit.point);
        }
        private void OnGroundClickSpawn(Vector3 posClicked)
        {
            var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), posClicked, Quaternion.identity);
            
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            Destroy(obj, 3);
        }*/
        private void DrawLineRendererPaths(NavMeshAgent agent, LineRenderer lineRenderer) //navigation path render line for in game *currently allocates 88 bytes per line drawn
        {
            
            if (agent.remainingDistance <= 0.1f || !_selectable.selected) //should only be visible when that unit is selected
            {
                lineRenderer.enabled = false;
                return;
            }

            var agentPath = agent.path;
            
            lineRenderer.positionCount = agentPath.corners.Length;
            lineRenderer.SetPositions(agentPath.corners);
            lineRenderer.enabled = true;
        }

        //ai
        private void UpdateAction(SmartObject smartObject)
        {
            if (smartObject == null || !performingForcedAction || !InRange(smartObject)) return;
            
            PerformAction(smartObject);
        }

        private SmartObject _tempSmartObject;
        private float _tempActionCounter;
        public void StartAction(SmartObject smartObject)
        {
            performingForcedAction = true;
            
            //cache the data
            _tempSmartObject = smartObject; 
            _tempActionCounter = smartObject.actionLength;
            
            smartObject.SetSmartObjectData(this);

            if (InRange(smartObject))
            {
                navMeshAgent.isStopped = true;
                PerformAction(smartObject);
                return;
            }
            
            MoveAgent(smartObject.objectCollider.ClosestPointOnBounds(transform.position));
        }

        private void PerformAction(SmartObject smartObject)
        {
            _tempActionCounter -= Time.deltaTime;
            
            //Debug.Log("Perform action.");
            //finish action
            if (_tempActionCounter <= 0)
            {
                
                return;
            }
            
            //reset in the end
            ResetAction();
        }

        private void MoveAgent(Vector3 targetPosition)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(targetPosition);
        }

        private void ResetAction()
        {
            performingForcedAction = false;
            _tempSmartObject = null;

        }
        
        public void PlayAnimation(string animName)
        {
            animator.SetTrigger(animName);
        }
        
        public void RotateTowardsObject()
        {
            //Vector3.RotateTowards(transform.)
        }

        private bool InRange(SmartObject smartObject)
        {
            return (smartObject.transform.position - transform.position).magnitude <= smartObject.stoppingDistance;
        }
    }
}
