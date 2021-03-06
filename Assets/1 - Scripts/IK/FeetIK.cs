﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectColoni
{
    public class FeetIK : MonoBehaviour
    {
        public bool enableFeetIk = true;
        public bool enableHandIk = true;
    
        [Header("HandIK")] 
        public Transform leftHandIkPosition;
        public Transform rightHandIkPosition;
        [Header("FootIK")]
        public float distanceToGround = 0.1f;
        public LayerMask groundLayer;
        [Header("Debug")] 
        public bool drawDebugRays;
        private Animator _animator;
        private static readonly int rightFootIk = Animator.StringToHash("RightFootIK");
        private static readonly int leftFootIk = Animator.StringToHash("LeftFootIK");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!_animator) return;

            if (enableHandIk)
            {
                //left hand
                if (leftHandIkPosition)
                {
                    _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                    _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIkPosition.position);
                    _animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIkPosition.rotation);
                }
                //right hand
                if (rightHandIkPosition)
                {
                    _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                    _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIkPosition.position);
                    _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIkPosition.rotation);
                }
            }

            if (enableFeetIk)
            {
                //set feet weights
                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                // _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _animator.GetFloat(rightFootIk));
                // _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _animator.GetFloat(rightFootIk));
                // _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat(leftFootIk));
                // _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat(leftFootIk));

                //left foot
                RaycastHit hit;
                Ray leftRay = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

                if (Physics.Raycast(leftRay, out hit, distanceToGround + 1f, groundLayer))
                {
                    var footPos = hit.point;
                    footPos.y += distanceToGround;
                    
                    _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
                    _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation);
                }
            
                if(drawDebugRays)
                    Debug.DrawRay(leftRay.origin, leftRay.direction, Color.red);
            
                //right foot
                Ray rightRay = new Ray(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

                if (Physics.Raycast(rightRay, out hit, distanceToGround + 1f, groundLayer))
                {
                    var footPos = hit.point;
                    footPos.y += distanceToGround;
                    
                    _animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
                    _animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation);
                }
            
                if(drawDebugRays)
                    Debug.DrawRay(rightRay.origin, rightRay.direction, Color.blue);

            }
        }
    }

}