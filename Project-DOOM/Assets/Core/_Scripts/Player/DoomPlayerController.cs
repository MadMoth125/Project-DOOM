using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using TNRD;
using UnityEngine;

public class DoomPlayerController : MonoBehaviour
{
	// public static event Action OnPlayerSpawned;
	
	// saving names for required input axis
	private const string Horizontal = "Horizontal";
	private const string Vertical = "Vertical";
	private const string MouseX = "Mouse X";
	private const string MouseY = "Mouse Y";
	
	[Header("Player References")]
	public SerializableInterface<ICharacterController> characterAsset;
	public SerializableInterface<ICameraController> cameraAsset;

	private ICharacterController _characterReference;
	private ICameraController _cameraReference;
	private CameraHeadBob _cameraHeadBob;
	
	[Space(10)]
	[Header("Spawn Parameters")]
	public Transform spawnPoint;
	
	[Space(10)]
	[Header("Camera Parameters")]
	public float cameraBobbingStrength = 0.1f;
	
	private void Awake()
	{
		_cameraHeadBob = new CameraHeadBob();
		
		if (!SpawnPlayer())
		{
			Debug.LogError($"{GetType()}: Failed to spawn player!");
			enabled = false;
		}
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		// getting input from the player in a normalized vector
		Vector2 movementInput = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)).normalized;
		
		_characterReference.MoveCharacter(new Vector3(movementInput.x, 0f, movementInput.y), // converting to Vector3
			_cameraReference.CameraTransform.rotation);

		_cameraHeadBob.SetMovementDirection(movementInput);
	}
	
	private void LateUpdate()
	{
		// getting input from the player in a raw vector
		Vector2 cameraInput = new Vector2(Input.GetAxisRaw(MouseX), Input.GetAxisRaw(MouseY));
		
		_cameraReference.RotateCamera(new Vector3(cameraInput.x, cameraInput.y, 0f));

		// applying camera bobbing/swaying to the camera's transform
		_cameraHeadBob.HandleCameraBobbing(_cameraReference.CameraComponent.transform, strength: cameraBobbingStrength);
	}
	
	public bool SpawnPlayer()
	{
		try
		{
			// instantiate character and camera, caching the references for later use
			_characterReference = Instantiate(characterAsset.Value.CharacterGameObject, spawnPoint.position,
				Quaternion.Euler(0f, spawnPoint.rotation.eulerAngles.y, 0f)).GetComponent<ICharacterController>();
		
			_cameraReference = Instantiate(cameraAsset.Value.CameraGameObject, spawnPoint.position,
				Quaternion.Euler(0f, spawnPoint.rotation.eulerAngles.y, 0f)).GetComponent<ICameraController>();
		
			if (_characterReference == null || _cameraReference == null) return false;
		}
		catch (Exception e)
		{
			// if instantiation fails, return false
			Debug.LogError($"{GetType()}: Failed to spawn player! Exception: {e}");
			return false;
		}
		
		_cameraReference.SetFollowTransform(_characterReference.CameraTransform);

		return true;
	}
}