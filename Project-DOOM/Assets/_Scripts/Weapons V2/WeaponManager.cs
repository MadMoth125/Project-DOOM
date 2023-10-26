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
	
	public List<Weapon> weapons;
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
		currentWeapon = Instantiate(weapons[0], _weaponParent);
		
		currentWeapon.transform.parent = _weaponParent;
		
		currentWeapon.transform.localPosition = Vector3.zero;
		currentWeapon.transform.localPosition += Vector3.forward * 1;
	}

	private void Update()
	{
		
	}

	private void LateUpdate()
	{
		
	}

	private void OnPlayerSpawned(ICharacterController characterRef, ICameraController cameraRef)
	{
		Debug.Log($"{GetType()}: Player Spawned");
		_weaponParent = cameraRef.CameraTransform;
	}
}