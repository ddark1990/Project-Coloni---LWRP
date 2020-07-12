using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiStats : MonoBehaviour
    {
        [Header("Stats")]
        [Range(0, 100)] public float health = 100;
        [Range(0, 100)] public float hunger = 100;
        [Range(0, 100)] public float thirst = 100;
        [Range(0, 100)] public float stamina = 100;
        [Range(0, 100)] public float energy = 100;
        [Range(0, 100)] public float comfort = 100;
        [Range(0, 100)] public float happiness = 100;
        
        [Header("Rates")]
        [Range(0, 10)] public float hungerRate;
        [Range(0, 10)] public float thirstRate;
        [Range(0, 10)] public float staminaRate;
        [Range(0, 10)] public float energyRate;
        
        private float _damageOverTimeTimer;

        public bool IsHungry => hunger <= 50;
        public bool IsThirsty => thirst <= 50;
        public bool IsFatigued => stamina <= 25;
        public bool IsTired => energy <= 20;
        public bool IsDead { get; set; }
        
        private void Update()
        {
            UpdateVitals();
        }
        
        private void UpdateVitals()
        {
            if (IsDead) return;

            DecreaseClampedFloat(hunger, hungerRate, out hunger);
            DecreaseClampedFloat(energy, energyRate, out energy);
            DecreaseClampedFloat(thirst, thirstRate, out thirst);

            if (hunger <= 0)
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
            health -= damage;
            Debug.Log(gameObject.name + " took " + damage + " damage.");
            
            if (health <= 0)
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

        
        [CreateAssetMenu(fileName = "VitalsDefault", menuName = "AI/DefaultVitalsObject")]
        public class AiVitalsObject : ScriptableObject
        {
            [Header("Default Values")]
            [Range(0, 1000)] public float health;
            [Range(0, 1000)] public float hydration;
            [Range(0, 1000)] public float hunger;
            [Range(0, 1000)] public float stamina;
            [Range(0, 1000)] public float energy;
            
            [Header("Rates")]
            public float hungerRate = 1;
            public float hydrationRate = 1;
            public float staminaRate = 1;
            public float energyRate = 1;
        }
    }
}
