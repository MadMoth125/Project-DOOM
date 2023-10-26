using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.Interfaces;
using UnityEngine;


namespace ProjectDOOM.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public event Action OnFireConditionMet;
	
        [SerializeField]
        protected GunControlParameters_SO gunControlParameters;
        protected IWeaponEventHandler eventHandler;

        protected bool[] fireConditions;

        protected virtual void Awake()
        {
            // eventHandler = GetComponent<IWeaponEventHandler>();
            eventHandler = GetComponentInChildren<IWeaponEventHandler>();
		
            if (eventHandler == null)
            {
                Debug.LogError($"No event handler found for {gameObject.name}.");
                return;
            }
            else
            {
                eventHandler.SetWeaponComponent(this);
            }
        }

        protected virtual void Update()
        {
            gunControlParameters.Fire(fireConditions, Fire);
        }

        protected virtual void Fire()
        {
            OnFireConditionMet?.Invoke();
        }
    }
}