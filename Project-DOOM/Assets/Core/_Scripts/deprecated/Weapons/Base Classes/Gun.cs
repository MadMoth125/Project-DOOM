using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.deprecated
{
	public class Gun : MonoBehaviour
	{
		public event Action OnFireActivated;
	
		[SerializeField]
		protected WeaponEventHandler weaponEventHandler;

		protected virtual void Awake()
		{
			// weaponEventHandler ??= GetComponent<SingleShotEventHandler>();
			// weaponEventHandler ??= GetComponentInChildren<SingleShotEventHandler>();
		
			weaponEventHandler.GunReference = this;
		}

		protected virtual void Update()
		{
		
		}
	
		/// <summary>
		/// Activates the <see cref="OnFireActivated"/> event from outside the <see cref="Gun"/> class
		/// </summary>
		protected void EventPlayerFired()
		{
			OnFireActivated?.Invoke();
		}
	}
}