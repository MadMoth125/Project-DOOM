using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{
    public bool CanFire { get; }
    
    public void SetWeaponComponent(Weapon weapon);
    public void EnableWeapon();
    public void DisableWeapon();
}