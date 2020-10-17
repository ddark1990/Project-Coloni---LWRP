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
            Pistol,
            Riffle,
            Bow,
            Hatchet,
            PickAxe,
            Hammer,
            Spear
        }

        public WeaponType weaponType;
        public string weaponTag; //pre much the name tag, defined in the WeaponModel class with the tag (not the item)

        public int damage;
        [Tooltip("If ranged.")] public int clipSize;
    }
}
