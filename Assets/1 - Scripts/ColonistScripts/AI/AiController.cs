﻿using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
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
        
        public SmartObject _activeSmartObject;
        public float _actionLength;
        public float _animationWaitTime;
        public bool _animationPlaying; //needed to run once in update loop
        private static readonly int ActionLength = Animator.StringToHash("actionLength");

        public LayerMask terrainHeightMask;


        private void OnEnable()
        {
            EventRelay.OnToggleDrafted += CancelAction;
        }
        
        private void Start()
        {
            OnStartInitializeComponents(this);
            InitializeSelectable();
            
            if(smartActions != null)
                smartActions.InitializeSmartActions(this);
            
        }
        
        private void Update()
        {
            if (selected && Input.GetKeyDown(KeyCode.Mouse1) && enablePlayerControl 
                && SelectionManager.HoveringObject == null) SetDestinationToMousePosition();
            
            if (selected && Input.GetKeyDown(KeyCode.Mouse1) && debugControlEnabled 
                && SelectionManager.HoveringObject == null) SetDestinationToMousePosition();
            
            //DrawLineRendererPaths(navMeshAgent, destinationLineRenderer); //fix for both ai
            if (!EventSystem.current.IsPointerOverGameObject()) OutlineHighlight();

            UpdateAction(_activeSmartObject);
            
            UpdateColonistTerrainHeight();
            
        }
        
        //ai ver 1
        private void UpdateAction(SmartObject smartObject) //work on switching actions/ canceling one out
        {
            if (smartObject != null && performingForcedAction && InRange(smartObject))
            {
                _animationWaitTime -= Time.deltaTime;
            }

            if (smartObject == null || !performingForcedAction || _animationWaitTime > 0) return;
            
            //what allows intervals between recurring actions logic
            if (_animationWaitTime < 0) _animationWaitTime = 0; //clamp down to 0

            PerformAction(smartObject);
        }
        
        private Vector3 _lastSmartObjectPosition;
        private bool _recurringAction;

        public void StartAction(SmartObject smartObject, bool recurring)
        {
            if(_activeSmartObject != null) ResetAction(_activeSmartObject); //clear agent first if anything from previous action still persisting 
            
            performingForcedAction = true;
            actionInProgress = true;

            _recurringAction = recurring;
            //cache the data
            _activeSmartObject = smartObject; 
            _actionLength = smartObject.actionLength;
            
            smartObject.SetSmartObjectData(this);

            if (InRange(smartObject)) return;
            
            Debug.Log("Going to object!");
            var position = smartObject.transform.position;
            
            MoveAgent(position);
            _lastSmartObjectPosition = position;
        }

        /// <summary>
        /// Performs the operation based on input per frame in the update loop.
        /// </summary>
        /// <param name="smartObject"></param>
        private void PerformAction(SmartObject smartObject)
        {
            if (!InRange(smartObject))
            {
                StopAgent(false);
                MoveAgent(smartObject.transform.position); //maybe
                return;
            }
            
            //when in range stop
            StopAgent(InRange(smartObject));
            
            if (_recurringAction && actionLength > 0)
            {
                _actionLength -= Time.deltaTime;
                animator.SetFloat(ActionLength, _actionLength);

                PlayAnimationTrigger(smartObject.animationTrigger);
                RotateTowardsObject(smartObject, rotSpeed);
                
                if(_actionLength <= 0) 
                {
                    ResetAnimation();
                    smartObject.activeAction.Act(this, smartObject);
                    _actionLength = smartObject.actionLength;
                }
                
                //Debug.Log("Recurring Action In Progress!");
                return;
            }

            if (!_recurringAction && _actionLength > 0)
            {
                _actionLength -= Time.deltaTime;
                animator.SetFloat(ActionLength, _actionLength);
                
                PlayAnimationTrigger(smartObject.animationTrigger);
                RotateTowardsObject(smartObject, rotSpeed);
                
                if(_actionLength <= 0)
                    smartObject.activeAction.Act(this, smartObject);
                
                //Debug.Log("Action In Progress!");
                return;
            }
            
            //Debug.Log("Finished Action!");
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
        public void ResetAction(SmartObject smartObject)
        {
            ResetAgent();
            
            //smartObject.ResetSmartObject();

            smartObject.beingUsed = false;
            performingForcedAction = false;
            _activeSmartObject = null;
            _animationPlaying = false;
            _recurringAction = false;
            actionInProgress = false;
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
            animator.SetFloat(ActionLength, 0);
        }

        
        private void PlayAnimationTrigger(string animTrigger)
        {
            if (_animationPlaying) return;
            
            animator.SetTrigger(animTrigger);
            Debug.Log(animTrigger);
            _animationPlaying = true;
        }
        
        private void StopAgent(bool active)
        {
            switch (aiType)
            {
                case AiType.Unity:
                    navMeshAgent.isStopped = active;

                    break;
                case AiType.AStar:
                    aiPath.isStopped = active;

                    break;
            }
        }

        public void ResetAnimation()
        {
            _animationPlaying = false;
        }

        private void CancelAction(SmartObject smartObject)
        {
            if (!actionInProgress) return;
            
            ResetAction(smartObject);
            Debug.Log("CanceledAction");
        }
        
        private void RotateTowardsObject(SmartObject smartObject, float rotationSpeed)
        {
            var lookRotation = Quaternion.identity;
            
            var step = Time.deltaTime * rotationSpeed;

            var direction = (smartObject.transform.position - transform.position).normalized;
            if(direction != Vector3.zero)
                lookRotation = Quaternion.LookRotation(direction);
            
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

        [Range(-5, 5)]public float heightOffset = -0.01f;
        private void UpdateColonistTerrainHeight() //move to IK controller, and change to update only when moving
        {
            //temp clamp for colonists rotating when picking up objects
            var a = transform.eulerAngles;
            a.x = Mathf.Clamp(a.x, 0, 0);
            transform.eulerAngles = a;
            
            Ray ray = new Ray(transform.position + new Vector3(0,2,0), Vector3.down);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            
            if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, terrainHeightMask))
            {
                var y = hitInfo.point.y/* + heightOffset*/;
                var transform1 = transform;
                var pos = transform1.position;
 
                pos.y = y;
 
                transform1.position = pos;
            }
        }

    }
}
