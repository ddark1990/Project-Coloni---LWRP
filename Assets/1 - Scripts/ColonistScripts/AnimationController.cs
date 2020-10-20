using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectColoni
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private float moveSpeedMultiplier = 1;
        [SerializeField] private float animSpeedMultiplier = 1;
        [SerializeField] [Range(.1f, 10f)]private float nonRootAnimSpeedMultiplier = 4.2f;
        [SerializeField] [Range(.1f, 10f)]private float nonRootIdleSpeedMultiplier = 5.5f;
        [SerializeField] private float turnSpeedMultiplier = 360;
        [SerializeField] private float stationaryTurnSpeed = 180;

        private AiController _controller;
        private float _forwardAmount;
        private float _turnAmount;

        private int _hash;
        private Animator _lastAnimatorCache; 
        private readonly Dictionary<string,int> _animatorParamCache = new Dictionary<string,int>( );

        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int MoveFaster = Animator.StringToHash("MoveFaster");
        private static readonly int DraftedState = Animator.StringToHash("DraftedState");
        private static readonly int DrawPistol = Animator.StringToHash("DrawPistol");
        private static readonly int CombatRangedMode = Animator.StringToHash("CombatRangedMode");
        private static readonly int CombatModeMelee = Animator.StringToHash("CombatModeMelee");

        
        private void Start()
        {
            _controller = GetComponent<AiController>();
        }

        private void Update()
        {
            switch (_controller.aiType)
            {
                case AiBase.AiType.Unity:
                    Move(_controller.navMeshAgent.remainingDistance > _controller.navMeshAgent.stoppingDistance
                        ? _controller.navMeshAgent.desiredVelocity
                        : Vector3.zero);

                    break;
                case AiBase.AiType.AStar:
                    switch (_controller.aiStats.stats.gender)
                    {
                        case Stats.Gender.Male:
                            Move(_controller.aiPath.remainingDistance > _controller.aiPath.endReachedDistance
                                ? _controller.aiPath.desiredVelocity
                                : Vector3.zero);

                            break;
                        case Stats.Gender.Female:
                            Move(_controller.aiPath.remainingDistance > _controller.aiPath.endReachedDistance
                                ? _controller.aiPath.desiredVelocity
                                : Vector3.zero);

                            break;
                        case Stats.Gender.Robot:
                            break;
                        case Stats.Gender.Alien:
                            break;
                        case Stats.Gender.Animal:
                            Move(_controller.aiPath.remainingDistance > _controller.aiPath.endReachedDistance
                                ? _controller.aiPath.desiredVelocity
                                : Vector3.zero);
                            break;
                    }

                    break;
            }
        }

        private void Move(Vector3 move)
        {
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            _forwardAmount = move.z;
            _turnAmount = Mathf.Atan2(move.x, move.z);

            switch (_controller.aiType)
            {
                case AiBase.AiType.Unity:

                    break;
                
                case AiBase.AiType.AStar:

                    switch (_controller.aiStats.stats.gender) 
                    {
                        case Stats.Gender.Male:
                            //_controller.aiPath.canMove = false;
                            _controller.aiPath.MovementUpdate(Time.deltaTime, out move, out var nextRotation1);
                            _controller.animator.applyRootMotion = true;

                            break;
                        case Stats.Gender.Female:
                            //_controller.aiPath.canMove = false;
                            _controller.aiPath.MovementUpdate(Time.deltaTime, out move, out var nextRotation2);
                            _controller.animator.applyRootMotion = true;

                            break;
                        case Stats.Gender.Robot:
                            break;
                        case Stats.Gender.Alien:
                            break;
                        case Stats.Gender.Animal:
                            //_controller.aiPath.canMove = true;
                            _controller.animator.applyRootMotion = false;

                            break;
                    }

                    break;
            }
            
            ApplyExtraTurnRotation();
            
            UpdateAnimator();
        }
        
        public void OnAnimatorMove()
        {
            Vector3 deltaPosition;
            Vector3 velocity;
            Vector3 v;
            switch (_controller.aiStats.stats.gender) 
            {
                case Stats.Gender.Male:
                    deltaPosition = _controller.animator.deltaPosition;
                    velocity = deltaPosition * moveSpeedMultiplier / Time.deltaTime;
            
                    v = (deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                    // we preserve the existing y part of the current velocity.
                    v.y = velocity.y;
                    velocity = v;
                    _controller.rigidBody.velocity = velocity;

                    break;
                case Stats.Gender.Female:
                    deltaPosition = _controller.animator.deltaPosition;
                    velocity = deltaPosition * moveSpeedMultiplier / Time.deltaTime;
            
                    v = (deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                    // we preserve the existing y part of the current velocity.
                    v.y = velocity.y;
                    velocity = v;
                    _controller.rigidBody.velocity = velocity;

                    break;
                case Stats.Gender.Robot:
                    break;
                case Stats.Gender.Alien:
                    break;
                case Stats.Gender.Animal:
                    break;
            }
        }
        
        private void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            var turnSpeed = Mathf.Lerp(stationaryTurnSpeed, turnSpeedMultiplier, _forwardAmount);
            transform.Rotate(0, _turnAmount * turnSpeed * Time.deltaTime, 0);
        }
        
        private void UpdateAnimator()
        {
            if( TryGetAnimatorParam(  _controller.animator, "Forward", out _hash ) ) 
            {
                _controller.animator.SetFloat(Forward, _forwardAmount);
            }
            if( TryGetAnimatorParam(  _controller.animator, "Turn", out _hash ) ) 
            {
                _controller.animator.SetFloat(Turn, _turnAmount);
            }
            if( TryGetAnimatorParam(  _controller.animator, "MoveFaster", out _hash ) ) 
            {
                _controller.animator.SetBool(MoveFaster, _controller.moveFaster);
            }
            if( TryGetAnimatorParam(  _controller.animator, "DraftedState", out _hash ) ) 
            {
                _controller.animator.SetBool(DraftedState, _controller.stateController.Drafted);
            }
            if( TryGetAnimatorParam(  _controller.animator, "CombatModeMelee", out _hash ) ) 
            {
                _controller.animator.SetBool(CombatModeMelee, _controller.combatController.combatModeMelee);
            }
            if( _controller.stateController.Drafted && 
                _controller.equipment.IsEquipped(EquipmentSlot.EquipmentType.RangedWep) && 
                TryGetAnimatorParam(  _controller.animator, "DrawPistol", out _hash ) ) 
            {
                _controller.animator.SetTrigger(DrawPistol);
            }
            
            switch (_controller.aiStats.stats.gender)
            {
                case Stats.Gender.Male:
                    if (_controller.rigidBody.velocity.magnitude > 0)
                    {
                        _controller.animator.speed = animSpeedMultiplier;
                    }
                    break;
                case Stats.Gender.Female:
                    if (_controller.rigidBody.velocity.magnitude > 0)
                    {
                        _controller.animator.speed = animSpeedMultiplier;
                    }
                    break;
                case Stats.Gender.Robot:
                    break;
                case Stats.Gender.Alien:
                    break;
                case Stats.Gender.Animal:
                    var animSpeed = _controller.aiPath.desiredVelocity.magnitude * nonRootAnimSpeedMultiplier;

                    if (_controller.aiPath.remainingDistance > _controller.aiPath.endReachedDistance)
                    {
                        _controller.animator.speed = animSpeed;
                    }
                    else
                    {
                        animSpeed /= nonRootIdleSpeedMultiplier;
                        _controller.animator.speed = animSpeed;

                    }

                    break;
            }
        }

        public void ActivateDrawWeaponAnim(Weapon.WeaponType weaponType)
        {
            _controller.animator.SetTrigger(Enum.GetName(typeof(Weapon.WeaponType), weaponType));
        }
        
        private bool TryGetAnimatorParam( Animator animator, string paramName, out int hash ) //caches and resolves the params with no GC allocation per param
        {
            if( (_lastAnimatorCache == null || _lastAnimatorCache != animator) && animator != null ) // Rebuild cache
            {
                _lastAnimatorCache = animator;
                _animatorParamCache.Clear( );
                foreach( var param in animator.parameters )
                {
                    var paramHash = Animator.StringToHash( param.name ); // could use param.nameHash property but this is clearer
                    _animatorParamCache.Add( param.name, paramHash );
                }
            }

            if( _animatorParamCache != null && _animatorParamCache.TryGetValue( paramName, out hash ) )
            {
                return true;
            }
            else
            {
                hash = 0;
                return false;
            }
        }
    }
}