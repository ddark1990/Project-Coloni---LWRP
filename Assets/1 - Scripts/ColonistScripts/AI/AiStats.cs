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
        [SerializeField] private Stats.AiStatsObject aiObjectStats;
        
        public BaseObjectData baseObjectInfo;
        public Stats stats;
        
        private float _damageOverTimeTimer;

        public bool IsHungry => stats.Food <= 50;
        public bool IsFatigued => stats.Stamina <= 25;
        public bool IsTired => stats.Energy <= 20;
        public bool IsDead { get; set; }

        private void Start()
        {
            InitializeStats();
            InitializeBaseObjectData();
        }
        
        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData("", "", null); //should generate random base stats, maybe
        }
        
        private void InitializeStats()
        {
            if (aiObjectStats != null)
            {
                stats = new Stats
                {
                    Age = aiObjectStats.age,
                    gender = aiObjectStats.gender,
                    Health = aiObjectStats.maxHealth,
                    MaxHealth = aiObjectStats.maxHealth,
                    Food = aiObjectStats.food,
                    Stamina = aiObjectStats.maxStamina,
                    MaxStamina = aiObjectStats.maxStamina,
                    Energy = aiObjectStats.energy,
                    EnableSecondaryStats = aiObjectStats.enableSecondaryStats,
                    Comfort = aiObjectStats.comfort,
                    Recreation = aiObjectStats.recreation,
                    HungerRate = aiObjectStats.hungerRate,
                    StaminaRate = aiObjectStats.staminaRate,
                    EnergyRate = aiObjectStats.energyRate
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

            Debug.Log("Died " + obj);
        }
    }
}
