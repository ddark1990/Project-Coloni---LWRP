using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(ItemData)", menuName = "ProjectColoni/Objects/Create Item/New Weapon", order = 4)]
    public class Weapon : ItemType
    {
        public enum WeaponType
        {
            Melee,
            Ranged,
            Tool
        }

        public WeaponType weaponType;

        public int damage;
        [Tooltip("If ranged.")] public int clipSize;
    }
}
