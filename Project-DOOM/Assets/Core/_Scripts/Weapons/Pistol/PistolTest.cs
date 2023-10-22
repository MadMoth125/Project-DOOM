using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.Weapons
{
    public class PistolTest : Weapon
    {
        private void Start()
        {
            // we initialize the fire conditions array with the number of conditions we want to check
            fireConditions = new bool[1] { false };
        }

        protected override void Update()
        {
            // we set the fire condition to the value of the event handler's CanFire property,
            // then we call the base Update method (which holds the logic for firing the weapon)
            fireConditions[0] = eventHandler.CanFire;
            base.Update();
        }

        protected override void Fire()
        {
            base.Fire();
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 50, Color.red, 1f);
        }
    }
}