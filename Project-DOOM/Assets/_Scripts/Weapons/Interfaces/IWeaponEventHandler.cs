using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.V2;
using UnityEngine;

namespace ProjectDOOM.Weapons.Interfaces
{
    public interface IWeaponEventHandler
    {
        public bool CanFire { get; }

        public void SetWeaponComponent(IWeapon weapon);
        public void EnableWeapon();
        public void DisableWeapon();
    }
}