using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiStats : MonoBehaviour
    {
        [Header("Ai Object Data")]
        [Tooltip("If null, will generate random values for the base data.")] 
        [SerializeField] private BaseScriptableData baseData;
        [SerializeField] private AiStatsObject statsData;
        
        public BaseObjectData baseObjectInfo;
        public Stats stats;

        private AiController _controller;
        
        private float _damageOverTimeTimer;

        public bool IsHungry => stats.Food <= 50;
        public bool IsFatigued => stats.Stamina <= 25;
        public bool IsTired => stats.Energy <= 20;
        public bool IsDead { get; set; }


        private void Awake()
        {
            InitializeStats();
        }

        private void Start()
        {
            InitializeBaseObjectData();

            _controller = GetComponent<AiController>();
            //Debug.Log(baseObjectInfo.Id);
        }
        
        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.spriteTexture) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null); //should generate random base stats, maybe
        }
        
        private void InitializeStats()
        {
            if (statsData != null)
            {
                stats = new Stats
                {
                    Age = statsData.age,
                    gender = statsData.gender,
                    Health = statsData.maxHealth,
                    MaxHealth = statsData.maxHealth,
                    Food = statsData.food,
                    Stamina = statsData.maxStamina,
                    MaxStamina = statsData.maxStamina,
                    Energy = statsData.energy,
                    EnableSecondaryStats = statsData.enableSecondaryStats,
                    Comfort = statsData.comfort,
                    Recreation = statsData.recreation,
                    HungerRate = statsData.hungerRate,
                    StaminaRate = statsData.staminaRate,
                    EnergyRate = statsData.energyRate
                }; 
            }
            else
            {
                stats = new Stats();
            }
        }
        
        private void Update()
        {
            UpdateVitals();
        }

        private void UpdateVitals()
        {
            if (IsDead) return;

            DecreaseClampedFloat(stats.Food, stats.HungerRate, out stats.Food);
            DecreaseClampedFloat(stats.Energy, stats.EnergyRate, out stats.Energy);

            if (stats.Stamina <= 0) _controller.moveFaster = false;

            switch (_controller.aiStats.stats.gender)
            {
                case Stats.Gender.Male:
                    if(_controller.moveFaster && _controller.rigidBody.velocity.magnitude > 0) stats.Stamina -= Time.deltaTime * stats.StaminaRate;

                    break;
                case Stats.Gender.Female:
                    if(_controller.moveFaster && _controller.rigidBody.velocity.magnitude > 0) stats.Stamina -= Time.deltaTime * stats.StaminaRate;

                    break;
                case Stats.Gender.Robot:
                    break;
                case Stats.Gender.Alien:
                    break;
                case Stats.Gender.Animal:
                    if(_controller.moveFaster && _controller.aiPath.velocity.magnitude > 0) stats.Stamina -= Time.deltaTime * stats.StaminaRate;
    
                    break;
            }
            if (stats.Stamina < statsData.maxStamina) stats.Stamina += Time.deltaTime * stats.StaminaRate / stats.StaminaRate;
            
            if (stats.Food <= 0)
            {
                TakeDamageOverTime(10,3);
            }
        }

        private void DecreaseClampedFloat(float floatToClamp, float rate, out float returnedFloat) //utility
        {
            if (floatToClamp > 0)
            {
                floatToClamp -= Time.deltaTime * rate;
                if (floatToClamp <= 0) floatToClamp = 0;
            }

            returnedFloat = floatToClamp;
        }

        public void TakeDamage(float damage)
        {
            stats.Health -= damage;
            Debug.Log(gameObject.name + " took " + damage + " damage.");
            
            if (stats.Health <= 0)
            {
                Die(this);
            }
        }

        public void TakeDamageOverTime(float damage, float waitTime)
        {
            _damageOverTimeTimer -= Time.deltaTime;

            if (!(_damageOverTimeTimer <= 0)) return;
            
            _damageOverTimeTimer = waitTime;
            TakeDamage(damage);
            
            
        }

        public void Die(object obj)
        {
            IsDead = true;

            _controller.animator.SetTrigger("Die");
            
            Debug.Log("Died " + obj);
        }
    }
}
