using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using Utilities;

[SelectionBase]
[RequireComponent(typeof(CharacterMotor))]
public class DoomCharacter : MonoBehaviour, IKinematicCharacterController, ICharacterController
{
	[field: SerializeField]
	public Transform CameraTransform { get; private set; }
	
	public CharacterMotor CharacterMotorComponent => _motor;
	private CharacterMotor _motor;

	public Transform CharacterTransform => transform;
	public GameObject CharacterGameObject => gameObject;
	public Vector3 Velocity => _motor.Velocity;
	
	[Header("Stable Movement")]
	public float maxStableMoveSpeed = 10f;
	public float stableMovementSharpness = 15;
	public float orientationSharpness = 10;

	[Header("Air Movement")]
	public float maxAirMoveSpeed = 10f;
	public float airAccelerationSpeed = 5f;
	public float drag = 0.1f;

	[Header("Misc")]
	public Vector3 gravity = new Vector3(0, -30f, 0);

	private Vector3 _moveInputVector = Vector3.zero;
	private Vector3 _lookInputVector = Vector3.zero;
	
	private void Awake()
	{
		_motor = gameObject.SearchForComponent<CharacterMotor>();
		
		_motor.CharacterController = this;
	}
	
	public void MoveCharacter(Vector3 input, Quaternion direction)
	{
		// Calculate camera direction and rotation on the character plane
		Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(direction * Vector3.forward, _motor.CharacterUp).normalized;
		
		if (cameraPlanarDirection.sqrMagnitude == 0f)
		{
			cameraPlanarDirection = Vector3.ProjectOnPlane(direction * Vector3.up, _motor.CharacterUp).normalized;
		}
		
		Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);

		// Move and look inputs
		_moveInputVector = cameraPlanarRotation * input;
		_lookInputVector = cameraPlanarDirection;
	}

	#region KinematicCharacterController Movement Handling

	public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
	{
		if (_lookInputVector == Vector3.zero) return;
		
		if (orientationSharpness > 0f)
		{
			// Smoothly interpolate from current to target look direction
			Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

			// Set the current rotation (which will be used by the KinematicCharacterMotor)
			currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
		}
		else
		{
			currentRotation = Quaternion.LookRotation(_lookInputVector, _motor.CharacterUp);
		}
	}

	public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
	{
		Vector3 targetMovementVelocity;
		
        if (_motor.GroundingStatus.IsStableOnGround)
        {
	        OnGroundMovement(ref currentVelocity, deltaTime);
        }
        else
        {
            InAirMovement(ref currentVelocity, deltaTime);

            // Gravity
            currentVelocity += gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (drag * deltaTime)));
        }

        void OnGroundMovement(ref Vector3 velocity, float delta)
        {
	        // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
	        velocity = _motor.GetDirectionTangentToSurface(velocity, _motor.GroundingStatus.GroundNormal) * velocity.magnitude;

	        // Calculate target velocity
	        Vector3 inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
	        Vector3 reorientedInput = Vector3.Cross(_motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
	        targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

	        // Smooth movement Velocity
	        velocity = Vector3.Lerp(velocity, targetMovementVelocity, 1 - Mathf.Exp(-stableMovementSharpness * delta));
        }

        void InAirMovement(ref Vector3 velocity, float delta)
        {
	        // Add move input
	        if (_moveInputVector.sqrMagnitude > 0f)
	        {
		        targetMovementVelocity = _moveInputVector * maxAirMoveSpeed;

		        // Prevent climbing on un-stable slopes with air movement
		        if (_motor.GroundingStatus.FoundAnyGround)
		        {
			        Vector3 perpendicularObstructionNormal = Vector3.Cross(
					        Vector3.Cross(
						        _motor.CharacterUp,
						        _motor.GroundingStatus.GroundNormal),
					        _motor.CharacterUp).normalized;
			        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpendicularObstructionNormal);
		        }

		        Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - velocity, gravity);
		        velocity += velocityDiff * (maxAirMoveSpeed * delta);
	        }
        }
	}

	#endregion
	
	#region KinematicCharacterController Functions

	public void BeforeCharacterUpdate(float deltaTime)
	{
		
	}

	public void PostGroundingUpdate(float deltaTime)
	{
		
	}

	public void AfterCharacterUpdate(float deltaTime)
	{
		
	}

	public bool IsColliderValidForCollisions(Collider coll)
	{
		return true;
	}

	public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
	{
		
	}

	public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
		ref HitStabilityReport hitStabilityReport)
	{
		
	}

	public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
		Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
	{
		
	}

	public void OnDiscreteCollisionDetected(Collider hitCollider)
	{
		
	}

	#endregion
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position + transform.up, transform.forward * 2f);
	}

	public void RotateCharacter(Vector3 deltaRotation)
	{
		SetCharacterRotation(Quaternion.Euler(transform.rotation.eulerAngles + deltaRotation));
	}

	public void SetCharacterPosition(Vector3 position)
	{
		CharacterMotorComponent.SetPosition(position);
	}

	public void SetCharacterRotation(Quaternion rotation)
	{
		CharacterMotorComponent.SetRotation(rotation);
	}
}