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
            if (smartObject == null || !performingForcedAction) return;

            PerformAction(smartObject);
        }

        private SmartObject _tempSmartObject;
        public float _tempActionCounter;
        public void StartAction(SmartObject smartObject)
        {
            //animator.ResetTrigger(animator.);

            performingForcedAction = true;
            
            //cache the data
            _tempSmartObject = smartObject; 
            _tempActionCounter = smartObject.actionLength;
            
            smartObject.SetSmartObjectData(this);

            if (InRange(smartObject)) return;
            
            Debug.Log("Going to object!");
            MoveAgent(smartObject.objectCollider.ClosestPointOnBounds(transform.position));
        }

        public float rotSpeed = 5;
        private void PerformAction(SmartObject smartObject)
        {
            if (!InRange(smartObject)) return;
            
            navMeshAgent.isStopped = true;
            
            _tempActionCounter -= Time.deltaTime;

            RotateTowardsObject(smartObject, rotSpeed);
            PlayAnimation(smartObject.animationTrigger);
            
            Debug.Log("Starting Action!");

            if (_tempActionCounter <= 0)
            {
                Debug.Log("Finished Action!");
                
                ResetAction(smartObject);
                return;
            }
            
        }

        private void MoveAgent(Vector3 targetPosition)
        {
            ResetAgent();
            
            navMeshAgent.SetDestination(targetPosition);
        }

        private void ResetAction(SmartObject smartObject)
        {
            ResetAgent();
            
            smartObject.ResetSmartObject();
            
            performingForcedAction = false;
            _tempSmartObject = null;
        }

        private void ResetAgent()
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = false;
            _animationStop = false;
        }

        private bool _animationStop;
        private void PlayAnimation(string animName)
        {
            if (_animationStop) return;
            
            animator.SetTrigger(animName);
            _animationStop = true;
        }

        private void CancelAnimation() //finish canceling animation 
        {
            Debug.Log("SHITFUCKASS");

        }
        
        private void RotateTowardsObject(SmartObject smartObject, float rotationSpeed)
        {
            var step = Time.deltaTime * rotationSpeed;

            var direction = (smartObject.transform.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, step);
        }

        private bool InRange(SmartObject smartObject)
        {
            return (smartObject.transform.position - transform.position).magnitude <= smartObject.stoppingDistance;
        }

        public AnimationClip GetRuntimeAnimationClipInfo(string animationName)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            var animationClip = new AnimationClip();
            
            foreach(var clip in clips)
            {
                if (clip.name.Equals(animationName))
                {
                    animationClip = clip;
                }
            }

            return animationClip;
        }
    }
}
