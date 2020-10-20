using System;
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

        public WeaponModel modelReference;
        public ModelReferenceTransform modelTransform;
        public WeaponType weaponType;
        
        [Serializable]
        public struct ModelReferenceTransform
        {
            public Vector3 position;
            public Vector3 rotation;
        }
        
        public int damage;
        [Tooltip("If ranged.")] public int clipSize;
    }
}
