using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class AiController : AiBase
    {
        [Header("Ai")]
        public bool performingForcedAction;
        public bool pickUpAvailable;
        public bool inAction;

        [Header("NavMesh Settings")] 
        public Vector3 navMeshSize = new Vector3(30,30,30);
        public float navMeshUpdateDistance = 5;

        private NavMeshSurface _surface;
        
        [Header("Settings")]
        public float rotSpeed = 5;
        
        [Header("Debug")]
        public bool playerControlled;
        
        public SmartObject _tempSmartObject;
        public float _tempActionCounter;
        public float _tempLogicCounter;
        public bool _animationPlay; //needed to run once in update loop
        private static readonly int ActionLength = Animator.StringToHash("actionLength");

        
        private void Start()
        {
            OnStartInitializeComponents();
            InitializeSelectable();
            
            CreateNavMeshSurface();
        }

        
        private void Update()
        {
            //if (selected && Input.GetKeyDown(KeyCode.Mouse1)) SetDestinationToMousePosition();
            
            DrawLineRendererPaths(navMeshAgent, destinationLineRenderer);
            if (!EventSystem.current.IsPointerOverGameObject()) OutlineHighlight();

            UpdateAction(_tempSmartObject);
            
            if (_surface != null)
            {
                NavMeshBuilder.UpdateNavMesh(_surface, transform.position, navMeshUpdateDistance);
            }
        }

        //ai ver 1
        private void UpdateAction(SmartObject smartObject) //work on switching actions/ canceling one out
        {
            if (smartObject == null || !performingForcedAction) return;
            
            PerformAction(smartObject);
        }

        
        public void StartAction(SmartObject smartObject)
        {
            if(_tempSmartObject != null) ResetAction(_tempSmartObject); //clear agent first if anything from previous action still persisting 
            
            performingForcedAction = true;

            //cache the data
            _tempSmartObject = smartObject; 
            _tempActionCounter = smartObject.actionLength;
            
            smartObject.SetSmartObjectData(this);

            if (InRange(smartObject)) return;
            
            //Debug.Log("Going to object!");
            //MoveAgent(smartObject.objectCollider.ClosestPointOnBounds(transform.position));
            MoveAgent(smartObject.transform.position);
        }
        private void PerformAction(SmartObject smartObject)
        {
            if (!InRange(smartObject)) return;
            
            navMeshAgent.isStopped = true;

            _tempActionCounter -= Time.deltaTime;
            animator.SetFloat(ActionLength, _tempActionCounter); 

            RotateTowardsObject(smartObject, rotSpeed);
            PlayAnimation(smartObject.animationTrigger);
            
            //Debug.Log("Starting Action!");
            inAction = true;
            
            smartObject.activeAction.Act(this, smartObject);
            
            if (!(_tempActionCounter <= 0)) return;
            
            inAction = false;
            Debug.Log("Finished Action!");

            ResetAction(smartObject);
        }
        private void MoveAgent(Vector3 targetPosition)
        {
            ResetAgent();
            
            navMeshAgent.SetDestination(targetPosition);
        }
        private void ResetAction(SmartObject smartObject)
        {
            ResetAgent();
            
            //smartObject.ResetSmartObject();
            
            smartObject.beingUsed = false;
            performingForcedAction = false;
            _tempSmartObject = null;
        }
        private void ResetAgent()
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = false;
            _animationPlay = false;
            inAction = false;
            animator.SetFloat(ActionLength, 0);
        }
        private void PlayAnimation(string animName)
        {
            if (_animationPlay) return;
            
            animator.SetTrigger(animName);
            _animationPlay = true;
        }

        private void CancelAction()
        {
            Debug.Log("CanceledAction");
        }

        #region SmartActionAi ver2


        #endregion
        
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
       
        
        private void SetDestinationToMousePosition()
        {
            if (cam == null) return;
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;

            MoveAgent(hit.point);
            OnGroundClickSpawn(hit.point);
        }
        private void OnGroundClickSpawn(Vector3 posClicked)
        {
            var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), posClicked, Quaternion.identity);
            
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            Destroy(obj, 3);
        }

        //navmesh
        private void CreateNavMeshSurface()
        {
            if (_surface != null) return;
            _surface = NavMeshBuilder.CreateNavSurface(this, navMeshSize).GetComponentInChildren<NavMeshSurface>();
            NavMeshBuilder.UpdateNavMesh(_surface, transform.position, navMeshUpdateDistance);
        }

        
    }
}
