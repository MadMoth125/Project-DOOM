using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.Weapons.V2
{
    public interface IWeapon
    {
	    public event Action OnFireConditionMet;
    }
}