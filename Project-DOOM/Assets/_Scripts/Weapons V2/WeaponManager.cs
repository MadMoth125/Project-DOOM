using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.Weapons.V2;
using UnityEngine;
using Utilities;

public class WeaponManager : MonoBehaviour
{
	private DoomPlayerController _owningController;
	private ICharacterController _owningCharacter;
	
	// public List<Weapon> weapons;
	public List<WeaponParameters> weaponParameters;
	[HideInInspector]
	public Weapon currentWeapon;

	private Transform _weaponParent;
	
	private void Awake()
	{
		_owningController = gameObject.SearchForComponent<DoomPlayerController>();
	}

	private void OnEnable()
	{
		DoomPlayerController.OnPlayerSpawned += OnPlayerSpawned;
	}
	
	private void OnDisable()
	{
		DoomPlayerController.OnPlayerSpawned -= OnPlayerSpawned;
	}

	private void Start()
	{
		// currentWeapon = Instantiate(weapons[0], _weaponParent);
		
		currentWeapon.transform.parent = _weaponParent;
		
		currentWeapon.transform.localPosition = Vector3.zero;
		currentWeapon.transform.localPosition += Vector3.forward * 1;
	}

	private void EquipWeapon(Weapon weapon)
	{
		if (weapon != null)
		{
			currentWeapon = Instantiate(weapon, _weaponParent);
		}
		currentWeapon = weapon;
	}
	
	private void OnPlayerSpawned(ICharacterController characterRef, ICameraController cameraRef)
	{
		_weaponParent = cameraRef.CameraTransform;
	}
}

[Serializable]
public struct WeaponParameters
{
	public Weapon weapon;
	public Vector3 offset;
	public int ammo;
	public int maxAmmo;
}