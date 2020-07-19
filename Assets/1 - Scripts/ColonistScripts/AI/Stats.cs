using System;
using UnityEngine;

namespace ProjectColoni
{
    public class Stats 
    {
        public enum Gender
        {
            Male = 0,
            Female,
            Robot,
            Alien
        }

        //primary info
        public string AiName = "PoonGalore";
        public string Description = "Pleb.";
        public int Age = 21;
        public Gender gender = Gender.Male;
            
        //primary stats
        public float Health = 100;
        public float Food = 100;
        public float Stamina = 100;
        public float Energy = 100;
        
        public bool EnableSecondaryStats = false;
        //secondary stats
        public float Comfort = 100;
        public float Recreation = 100;
        
        //rates
        public float HungerRate = .5f;
        public float StaminaRate = .5f;
        public float EnergyRate = .2f;

        
        [CreateAssetMenu(fileName = "AiStats", menuName = "AI/AiStats")]
        public class AiStatsObject : ScriptableObject //pre made Ai Stats using scriptable objects
        {
            public enum Gender
            {
                Male,
                Female,
                Robot,
                Alien
            }

            [Header("Info")] 
            public string aiName;
            [TextArea] public string description;
            public int age;
            public Stats.Gender gender;
            
            [Header("Vital Stats")]
            [Range(0, 100)] public float health;
            [Range(0, 100)] public float food;
            [Range(0, 100)] public float stamina;
            [Range(0, 100)] public float energy;

            [Header("Secondary Stats")] 
            public bool enableSecondaryStats;
            [Range(0, 100)] public float comfort;
            [Range(0, 100)] public float recreation;

            [Header("Rates")]
            public float hungerRate;
            public float staminaRate;
            public float energyRate;
        }

    }
}

