using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class AiVitals : MonoBehaviour
    {
        [Range(0, 1000)] public float health;
        [Range(0, 1000)] public float hunger;
        [Range(0, 1000)] public float thirst;
        [Range(0, 1000)] public float stamina;
        [Range(0, 1000)] public float energy;

        public AiVitalsObject defaultVitals;

        public bool IsHungry => hunger <= 50;
        public bool IsThirsty => thirst <= 50;
        public bool IsFatigued => stamina <= 25;
        public bool IsTired => energy <= 20;
        public bool isDead;

        private void Awake()
        {
            InitializeVitals();
        }

        private void Update()
        {
            UpdateVitals();
        }

        private void InitializeVitals()
        {
            health = defaultVitals.health;
            thirst = defaultVitals.hydration;
            hunger = defaultVitals.hunger;
            stamina = defaultVitals.stamina;
            energy = defaultVitals.energy;
        }
        
        private void UpdateVitals()
        {
            if (isDead) return;

            DecreaseClampedFloat(hunger, defaultVitals.hungerRate, out hunger);
            DecreaseClampedFloat(energy, defaultVitals.energyRate, out energy);
            DecreaseClampedFloat(thirst, defaultVitals.hydrationRate, out thirst);

            if (hunger <= 0)
            {
                TakeDamageOverTime(10,3);
            }
        }

        private void DecreaseClampedFloat(float floatToClamp, float rate, out float returnedFloat)
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

        private float _damageOverTimeTimer;
        public void TakeDamageOverTime(float damage, float waitTime)
        {
            _damageOverTimeTimer -= Time.deltaTime;

            if (!(_damageOverTimeTimer <= 0)) return;
            
            _damageOverTimeTimer = waitTime;
            TakeDamage(damage);
        }

        public void Die(object obj)
        {
            isDead = true;

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
