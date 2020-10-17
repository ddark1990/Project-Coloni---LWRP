using System;
using UnityEngine;

namespace ProjectColoni
{
    public class WeaponHolderRelay : MonoBehaviour
    {
        //public Transform rightHand; //move into IK controller later
        public WeaponModel[] weaponModels;

        public void EnableWeaponModel(Item item, Weapon.WeaponType weaponType, string weaponTag)
        {
            var weapon = item.itemTypeData as Weapon;
            
        }
    }
}
