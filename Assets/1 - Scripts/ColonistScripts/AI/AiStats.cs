using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiStats : MonoBehaviour
    {
        //add base stats 
        public Stats.AiStatsObject objectStats;
        public Stats stats;
        
        private float _damageOverTimeTimer;

        public bool IsHungry => stats.Food <= 50;
        public bool IsFatigued => stats.Stamina <= 25;
        public bool IsTired => stats.Energy <= 20;
        public bool IsDead { get; set; }

        private void Start()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            if (objectStats != null)
            {
                stats = new Stats
                {
                    Age = objectStats.age,
                    gender = objectStats.gender,
                    Health = objectStats.maxHealth,
                    MaxHealth = objectStats.maxHealth,
                    Food = objectStats.food,
                    Stamina = objectStats.maxStamina,
                    MaxStamina = objectStats.maxStamina,
                    Energy = objectStats.energy,
                    EnableSecondaryStats = objectStats.enableSecondaryStats,
                    Comfort = objectStats.comfort,
                    Recreation = objectStats.recreation,
                    HungerRate = objectStats.hungerRate,
                    StaminaRate = objectStats.staminaRate,
                    EnergyRate = objectStats.energyRate
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
