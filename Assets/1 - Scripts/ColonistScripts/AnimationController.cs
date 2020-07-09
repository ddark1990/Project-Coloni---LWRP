using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private float moveSpeedMultiplier = 1f;
        [SerializeField] private float animSpeedMultiplier = 1f;

        private int _hash;
        private Animator _lastAnimatorCache; 
        private readonly Dictionary<string,int> _animatorParamCache = new Dictionary<string,int>( );

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            
        }
        
        /*public void OnAnimatorMove()
        {
             _playerController.playerRigidBody.velocity = _animator.deltaPosition * moveSpeedMultiplier / Time.deltaTime; //apply root motion control
        }
        
        private void UpdateAnimator() //start updating all the animation states
        {
            if( TryGetAnimatorParam( _animator, "Vertical", out _hash ) ) //up and down
            {
                //_animator.SetFloat(Vertical, _playerController.axisInput.y);
            }
            if( TryGetAnimatorParam( _animator, "Horizontal", out _hash ) ) //left and right
            {
                //_animator.SetFloat(Horizontal, _playerController.axisInput.x);
            }

            if (_playerController.playerRigidBody.velocity.magnitude > 0)
            {
                _animator.speed = animSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                _animator.speed = 1;
            }
        }*/
        
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