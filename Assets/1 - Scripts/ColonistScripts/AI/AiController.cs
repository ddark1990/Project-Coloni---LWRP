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
        public bool moveFaster;
        public bool performingForcedAction;
        public bool pickUpAvailable;
        public bool actionInProgress;
        public bool playerOwned;
        public bool enablePlayerControl;
        
        [Header("Settings")]
        public float rotSpeed = 5;

        [Header("Debug")] 
        public bool debugControlEnabled;
        
        public SmartObject _tempSmartObject;
        public float _tempActionCounter;
        public float _tempLogicCounter;
        public bool _animationPlay; //needed to run once in update loop
        private static readonly int ActionLength = Animator.StringToHash("actionLength");

        public LayerMask terrainHeightMask;
        
        
        private void Start()
        {
            OnStartInitializeComponents(this);
            InitializeSelectable();
        }
        
        private void Update()
        {
            if (selected && Input.GetKeyDown(KeyCode.Mouse1) && enablePlayerControl || debugControlEnabled) SetDestinationToMousePosition();
            
            //DrawLineRendererPaths(navMeshAgent, destinationLineRenderer); //fix for both ai
            if (!EventSystem.current.IsPointerOverGameObject()) OutlineHighlight();

            UpdateAction(_tempSmartObject);
            
            UpdateColonistTerrainHeight();
            UpdateColonistState();
        }

        private void UpdateColonistState()
        {
            if (stateController.Drafted)
            {
                
            }
        }

        private void UpdateColonistTerrainHeight() //move to IK controller, and change to update only when moving
        {
            //temp clamp for colonists rotating when picking up objects
            var a = transform.eulerAngles;
            a.x = Mathf.Clamp(a.x, 0, 0);
            transform.eulerAngles = a;
            
            Ray ray = new Ray(transform.position + new Vector3(0,2f,0), new Vector3(0,-2,0));
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            
            if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, terrainHeightMask))
            {
                var y = hitInfo.point.y;
                var transform1 = transform;
                var pos = transform1.position;
 
                pos.y = y;
 
                transform1.position = pos;
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
            
            switch (aiType)
            {
                case AiType.Unity:
                    navMeshAgent.isStopped = true;

                    break;
                case AiType.AStar:
                    aiPath.isStopped = true;

                    break;
            }

            _tempActionCounter -= Time.deltaTime;
            animator.SetFloat(ActionLength, _tempActionCounter); 

            if(!InRange(smartObject)) RotateTowardsObject(smartObject, rotSpeed);

            PlayAnimation(smartObject.animationTrigger);
            
            //Debug.Log("Starting Action!");
            actionInProgress = true;
            
            smartObject.activeAction.Act(this, smartObject);
            
            if (!(_tempActionCounter <= 0)) return;
            
            actionInProgress = false;
            Debug.Log("Finished Action!");

            ResetAction(smartObject);
        }
        private void MoveAgent(Vector3 targetPosition)
        {
            ResetAgent();

            switch (aiType)
            {
                case AiType.Unity:
                    navMeshAgent.SetDestination(targetPosition);

                    break;
                case AiType.AStar:
                    aiPath.destination = targetPosition;
                    
                    break;
            }
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
            switch (aiType)
            {
                case AiType.Unity:
                    navMeshAgent.ResetPath();
                    navMeshAgent.isStopped = false;

                    break;
                case AiType.AStar:
                    aiPath.destination = transform.position;
                    aiPath.isStopped = false;

                    break;
            }
            _animationPlay = false;
            actionInProgress = false;
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
            //OnGroundClickSpawn(hit.point);
        }
        private void OnGroundClickSpawn(Vector3 posClicked)
        {
            var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), posClicked, Quaternion.identity);
            
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            Destroy(obj, 3);
        }
    }
}
