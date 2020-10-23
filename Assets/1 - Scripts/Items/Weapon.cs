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

        public enum AmmoType
        {
            Pistol,
            Riffle,
            Energy
        }
        
        [Header("Weapon Data")]
        public WeaponModel modelReference;
        public ModelReferenceTransform modelTransform;
        public WeaponType weaponType;
        public AmmoType ammoType;

        [Header("Animation")] 
        public string animationAttackTrigger;
        public string animationDrawTrigger;
        
        [Serializable]
        public struct ModelReferenceTransform
        {
            public Vector3 position;
            public Vector3 rotation;
        }

        [Range(0, 100)] public float attackRange;
        [Range(0, 25)] public float attackSpeed;
        [Range(0, 25)] public float reloadSpeed;
        public int damage;
        public int clipSize;

        [Serializable]
        public struct AudioData
        {
            public AudioClip attackSound;
            public AudioClip hitSound;
        }
        [Serializable]
        public struct ParticleData
        {
            public ParticleSystem muzzleFlash;
            public ParticleSystem impact;
            public ParticleSystem bulletEject;
        }
        
        [Header("Misc Data")]
        public AudioData audioData;
        public ParticleData particleData;
    }
}
