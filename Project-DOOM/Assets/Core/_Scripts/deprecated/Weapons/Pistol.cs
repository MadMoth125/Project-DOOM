using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.deprecated
{
	public class Pistol : Gun
	{
		private SingleShotEventHandler _singleShotEventHandler;
	
		protected override void Awake()
		{
			base.Awake();
		
			// cast the weapon event handler to a single shot event handler
			// _singleShotEventHandler = weaponEventHandler as SingleShotEventHandler;
		}

		protected override void Update()
		{
			// if (Input.GetMouseButtonDown(0) && (!_singleShotEventHandler.IsFiring && _singleShotEventHandler.CanFire))
			// {
			// 	EventPlayerFired();
			// }
		}
	}
}