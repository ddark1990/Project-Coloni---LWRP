using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiSensors : MonoBehaviour
    {
        public Transform focus;
        [Space]
        [Range(0, 100)] public float radius = 10;
        [Range(0, 360)] public float viewAngle;
        [Range(0, 2)]public float rayHeight = 1.6f;
        [Space]

        public LayerMask layerMask;
        public int initialArrayCount = 25;
        public Collider[] overlapSphereNonAllocResult;

        private int _numOfHits;
        private bool _foundTarget;
        
        
        private void Awake()
        {
            CreateSensors();
        }

        private void Update()
        {
            //UpdateOverlapSphereNonAlloc();
            
        }

        private void CreateSensors()
        {
            overlapSphereNonAllocResult = new Collider[initialArrayCount];
            _closestCollider = new Collider();
        }
        
        private void UpdateOverlapSphereNonAlloc()
        {
            if (overlapSphereNonAllocResult.Length.Equals(0)) return;

            FindTargetsInView();
        }

        private void OverlapSphereCastNonAlloc() 
        {    
            Array.Clear(overlapSphereNonAllocResult, 0, overlapSphereNonAllocResult.Length);
            
            _numOfHits = Physics.OverlapSphereNonAlloc(transform.position, radius, overlapSphereNonAllocResult, layerMask);
        }


        private void FindTargetsInView() //prolly not gonna need to have a cone view unless for some sort of patrolling behaviour
        {
            OverlapSphereCastNonAlloc(); 

            for (var i = 0; i < _numOfHits; i++)
            {
                focus = FindClosestTransform(overlapSphereNonAllocResult, transform);

                if (focus == null) return;
                
                var dirToTarget = (focus.position - transform.position).normalized;

                if (!(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)) continue;

                var distToTarget = Vector3.Distance(transform.position, focus.position);

                if (Physics.Raycast(transform.position + new Vector3(0, rayHeight, 0),
                    dirToTarget + new Vector3(0, rayHeight, 0), distToTarget, layerMask))
                    continue; //change target mask when obstacles are present


                if (!_foundTarget && focus != transform) //if target is not us and we haven't found one already
                {
                    //ai has vision of the object within his field of view
                    _foundTarget = true;

                    //targetInViewEvent(target); //found target
                    //Debug.Log(target.name + " In View", this);
                }
                else _foundTarget = false;
            }

        }

        private Collider _closestCollider;
        
        private Transform FindClosestTransform(Collider[] array, Transform compare)
        {
            var closestDist = 0f;
        
            foreach (var tempCollider in array)
            {
                if (tempCollider == null) break;

                if (_closestCollider == null && tempCollider.transform != compare) 
                {
                    // first one, so choose it for now
                    _closestCollider = tempCollider;
                    closestDist = (tempCollider.transform.position - compare.position).magnitude;
                } 
                else if (tempCollider.transform != compare)
                {
                    // is this one closer than the last?
                    var dist = (tempCollider.transform.position - compare.position).magnitude;

                    if (!(dist < closestDist)) continue;
                
                    // we found a closer one, use it
                    _closestCollider = tempCollider;
                    closestDist = dist;
                }
            }

            return _closestCollider == null ? null : _closestCollider.transform;
        }
        
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
