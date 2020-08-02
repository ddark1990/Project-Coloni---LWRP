using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(StatsData)", menuName = "ProjectColoni/Objects/AI/New AiStats", order = 4)]
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
        public int age;
        public Stats.Gender gender;
            
        [Header("Vital Stats")]
        [Range(0, 500)] public float maxHealth;
        [Range(0, 500)] public float maxStamina;
        [Range(0, 100)] public float food;
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
