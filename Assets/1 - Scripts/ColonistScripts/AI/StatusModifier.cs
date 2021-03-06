﻿using System;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "StatusModifier", menuName = "ProjectColoni/Objects/AI/New StatusModifier", order = 2)]
    public class StatusModifier : StatusNotification
    {
        public bool stackable;
        public int stackLimit;
        public float modifierTimer;
        public Modifier[] modifiers;

        [Serializable]
        public struct Modifier 
        {
            public enum Stat
            {
                Health,
                Stamina,
                Energy,
                Food,
                Comfort,
                Recreation,
                MaxHealth,
                MaxStamina,
                MaxEnergy,
                MaxFood,
                MovementSpeed
            }
            public Stat modifiedStat;
            [Range(-100, 100)] public float modifierValue;
        }
    }
}