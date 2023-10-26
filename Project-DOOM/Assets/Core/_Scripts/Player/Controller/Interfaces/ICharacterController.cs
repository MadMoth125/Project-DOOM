using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public interface ICharacterController
{
	/// <summary>
	/// Returns the <see cref="CharacterMotor"/> component of the character.
	/// </summary>
	public CharacterMotor CharacterMotorComponent { get; }
	
	/// <summary>
	/// Returns a <see cref="Transform"/> to an ideal position for the camera to follow.
	/// </summary>
	public Transform CameraTransform { get; }
	
	/// <summary>
	/// Returns the <see cref="Transform"/> of the character.
	/// </summary>
	public Transform CharacterTransform { get; }
	
	/// <summary>
	/// Returns the <see cref="GameObject"/> of the character.
	/// </summary>
	public GameObject CharacterGameObject { get; }

	public void MoveCharacter(Vector3 input, Quaternion direction);
	public void RotateCharacter(Vector3 deltaRotation);
	
	public void SetCharacterPosition(Vector3 position);
	public void SetCharacterRotation(Quaternion rotation);
}