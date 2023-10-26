using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using TNRD;
using UnityEngine;

public class DoomPlayerManager : MonoBehaviour
{
	public static readonly List<IPlayerInterface> PlayerReferences = new List<IPlayerInterface>();
	
	private const string Horizontal = "Horizontal";
	private const string Vertical = "Vertical";
	
	private const string MouseX = "Mouse X";
	private const string MouseY = "Mouse Y";
	
	public SerializableInterface<ICameraInterface> playerCamera;
	public SerializableInterface<IPlayerInterface> playerCharacter;
	
	private CameraHeadBob _cameraHeadBob;
	
	[Space(10)]
	[Header("Spawn Parameters")]
	public Transform spawnPoint;
	
	[Space(10)]
	[Header("Camera Parameters")]
	public Transform cameraTarget;
	public float cameraBobbingStrength = 0.1f;
	
	private void Awake()
	{
		_cameraHeadBob = new CameraHeadBob();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		SpawnCharacter();
	}

	private void Update()
	{
		Vector2 movementInput = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical));
		
		characterController.HandleMovementInput(
			movementInput.normalized,
			cameraController.CameraReference.transform.rotation);
		
		_cameraHeadBob.SetMovementDirection(movementInput);
	}

	private void LateUpdate()
	{
		playerCharacter.HandleCameraInput(
			new Vector2(Input.GetAxisRaw(MouseX), 0f /* Input.GetAxisRaw(MouseY) */),
			cameraTarget);
		
		_cameraHeadBob.HandleCameraBobbing(cameraController.CameraReference.transform, strength: cameraBobbingStrength);
	}

	public void SpawnCharacter()
	{
		PlayerReferences.Add(Instantiate(
			playerCharacter,
			spawnPoint.position,
			Quaternion.Euler(0f, spawnPoint.rotation.eulerAngles.y, 0f)));
	} 
}