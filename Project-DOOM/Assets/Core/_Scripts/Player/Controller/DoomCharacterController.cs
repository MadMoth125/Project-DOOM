using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(KinematicCharacterMotor))]
public class DoomCharacterController : MonoBehaviour , ICharacterController
{
	private KinematicCharacterMotor _motor;

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
		// lazy load if null
		_motor ??= GetComponent<KinematicCharacterMotor>();
		_motor ??= GetComponentInChildren<KinematicCharacterMotor>();
		
		_motor.CharacterController = this;
	}

	/// <summary>
	/// This is called every frame by DoomPlayerManager
	/// </summary>
	public void HandleMovementInput(Vector2 direction, Quaternion rotation)
	{
		// Clamp input
		Vector3 moveInputVector = new Vector3(direction.x, 0f, direction.y);

		// Calculate camera direction and rotation on the character plane
		Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(rotation * Vector3.forward, _motor.CharacterUp).normalized;
		if (cameraPlanarDirection.sqrMagnitude == 0f)
		{
			cameraPlanarDirection = Vector3.ProjectOnPlane(rotation * Vector3.up, _motor.CharacterUp).normalized;
		}
		Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);

		// Move and look inputs
		_moveInputVector = cameraPlanarRotation * moveInputVector;
		_lookInputVector = cameraPlanarDirection;
	}
	
	public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
	{
		if (_lookInputVector != Vector3.zero && orientationSharpness > 0f)
		{
			// Smoothly interpolate from current to target look direction
			Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

			// Set the current rotation (which will be used by the KinematicCharacterMotor)
			currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
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

        void OnGroundMovement(ref Vector3 curVel, float d)
        {
	        // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
	        curVel = _motor.GetDirectionTangentToSurface(curVel, _motor.GroundingStatus.GroundNormal) * curVel.magnitude;

	        // Calculate target velocity
	        Vector3 inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
	        Vector3 reorientedInput = Vector3.Cross(_motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
	        targetMovementVelocity = reorientedInput * maxStableMoveSpeed;

	        // Smooth movement Velocity
	        curVel = Vector3.Lerp(curVel, targetMovementVelocity, 1 - Mathf.Exp(-stableMovementSharpness * d));
        }

        void InAirMovement(ref Vector3 curVel, float d)
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

		        Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - curVel, gravity);
		        curVel += velocityDiff * (maxAirMoveSpeed * d);
	        }
        }
	}

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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position + transform.up, transform.forward * 2f);
	}
}