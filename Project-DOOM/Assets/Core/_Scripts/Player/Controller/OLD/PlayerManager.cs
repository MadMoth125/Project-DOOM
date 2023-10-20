using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doom.DeadScripts
{
	public class PlayerManager : MonoBehaviour
	{
		private const string Horizontal = "Horizontal";
		private const string Vertical = "Vertical";
		private const string MouseX = "Mouse X";
		private const string MouseY = "Mouse Y";
	
	
		public DoomCamera playerCamera;
		public DoomController playerController;

		private void Awake()
		{
			// if player references are null, try to find them in the scene
			playerCamera ??= FindObjectOfType<DoomCamera>();
			playerController ??= FindObjectOfType<DoomController>();
		}

		private void OnEnable()
		{
		
		}

		private void OnDisable()
		{
		
		}

		private void Start()
		{
		
		}

		private void Update()
		{
			CharacterInputs characterInputs = new CharacterInputs();

			// Build the CharacterInputs struct
			characterInputs.MoveAxisForward = Input.GetAxisRaw(Vertical);
			characterInputs.MoveAxisRight = Input.GetAxisRaw(Horizontal);
			characterInputs.CameraRotation = playerCamera.transform.rotation;
			characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
			characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
			characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

			// Apply inputs to character
			playerController.SetInputs(ref characterInputs);
		}
	}
}
