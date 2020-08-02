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
        public int Age = 21;
        public Gender gender = Gender.Male;
            
        //primary stats
        public float Health = 100;
        public float MaxHealth = 100;
        public float Stamina = 100;
        public float MaxStamina = 100;
        public float Food = 100;
        public float Energy = 100;
        
        public bool EnableSecondaryStats = false;
        //secondary stats
        public float Comfort = 100;
        public float Recreation = 100;
        
        //rates
        public float HungerRate = .5f;
        public float StaminaRate = .5f;
        public float EnergyRate = .2f;
    }
}

