using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class DoomPlayerManager : MonoBehaviour
{
	private const string Horizontal = "Horizontal";
	private const string Vertical = "Vertical";
	
	private const string MouseX = "Mouse X";
	private const string MouseY = "Mouse Y";
	
	public DoomCharacterController characterController;
	public DoomCameraController cameraController;
	private KinematicCharacterMotor characterMotor;
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
		// Finding references in scene if null
		characterController ??= FindObjectOfType<DoomCharacterController>();
		cameraController ??= FindObjectOfType<DoomCameraController>();
		
		// Getting a reference to the KinematicCharacterMotor on the character
		characterMotor = characterController.gameObject.SearchForComponent<KinematicCharacterMotor>();
		
		_cameraHeadBob = new CameraHeadBob();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		characterMotor.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
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
		cameraController.HandleCameraInput(
			new Vector2(Input.GetAxisRaw(MouseX), 0f /* Input.GetAxisRaw(MouseY) */),
			cameraTarget);
		
		_cameraHeadBob.HandleCameraBobbing(cameraController.CameraReference.transform, strength: cameraBobbingStrength);
	}
}