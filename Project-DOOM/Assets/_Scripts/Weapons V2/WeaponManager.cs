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
	public List<WeaponParameters> weaponList;
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
		EquipWeapon(ref weaponList, 0);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			EquipWeapon(ref weaponList, 0);
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			EquipWeapon(ref weaponList, 1);
		}
	}

	private void EquipWeapon(ref List<WeaponParameters> weapons, int index)
	{
		// if the index is out of range, return
		// if the weapon in the slot is null, return
		if ((index < 0 || index > weapons.Count) || weapons[index].weapon == null)
		{
			Debug.LogWarning($"No weapon in slot {index}");
			return;
		}

		if (currentWeapon != null && currentWeapon.fireConditionals[0] == false)
		{
			Debug.LogWarning("Cannot switch weapons while firing");
			return;
		}
		
		var weaponStructVar = weapons[index];

		// Store the remaining ammo of the current weapon
		int remainingAmmo = (currentWeapon != null) ? currentWeapon.ammo : 0;

		// if the current weapon is not null, destroy it
		if (currentWeapon != null)
		{
			Destroy(currentWeapon.gameObject);
		}

		// instantiate the weapon in the slot with appropriate offset
		currentWeapon = Instantiate(weaponStructVar.weapon, _weaponParent);
		currentWeapon.transform.localPosition = weaponStructVar.offset;

		// set the "default" amount of ammo
		// if the weapon has not been picked up before.
		if (!weaponStructVar.hasBeenPickedUp)
		{
			currentWeapon.ammo = weaponStructVar.weapon.maxAmmo;
			weaponStructVar.hasBeenPickedUp = true; // set the weapon to have been "picked up"
		}
		else // otherwise, set the ammo to the amount it had before
		{
			currentWeapon.ammo = Mathf.Min(remainingAmmo, weaponStructVar.weapon.maxAmmo);
		}

		// Update the weapon in the list with the new struct
		weapons[index] = weaponStructVar;
	}

	private void UpdateWeaponParameters()
	{
		
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
	[HideInInspector]
	public bool hasBeenPickedUp;
}