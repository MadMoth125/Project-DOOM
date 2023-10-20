using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace Doom.DeadScripts
{
	[RequireComponent(typeof(KinematicCharacterMotor))]
	public class DoomController : MonoBehaviour, ICharacterController
	{
		private KinematicCharacterMotor _kinematicMotor;
		
		[Header("Stable Movement")]
		public float maxStableMoveSpeed = 10f;
		public float stableMovementSharpness = 15f;
		
		[Header("Air Movement")]
		public float maxAirMoveSpeed = 15f;
		public float airAccelerationSpeed = 15f;
		public float drag = 0.1f;

		[Header("Jumping")]
		public bool allowJumpingWhenSliding = false;
		public float jumpUpSpeed = 10f;
		public float jumpScalableForwardSpeed = 10f;
		public float jumpPreGroundingGraceTime = 0f;
		public float jumpPostGroundingGraceTime = 0f;

		[Header("Misc")]
		public Vector3 gravity = new Vector3(0, -30f, 0);

		private Vector3 _moveInputVector;
		private Vector3 _lookInputVector;
		
		private bool _jumpRequested = false;
		private bool _jumpConsumed = false;
		private bool _jumpedThisFrame = false;
		
		private float _timeSinceJumpRequested = Mathf.Infinity;
		private float _timeSinceLastAbleToJump = 0f;
		
		private Vector3 _internalVelocityAdd = Vector3.zero;
		
		private bool _shouldBeCrouching = false;
		private bool _isCrouching = false;

		private void Awake()
		{
			// lazy load the kinematic motor reference
			_kinematicMotor ??= GetComponent<KinematicCharacterMotor>();
			_kinematicMotor ??= GetComponentInChildren<KinematicCharacterMotor>();
			
			// set the character controller reference
			_kinematicMotor.CharacterController = this;
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
			
		}

		public void SetInputs(ref CharacterInputs inputs)
	        {
	            // Clamp input
	            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);
	            
	            // Calculate camera direction and rotation on the character plane
	            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, _kinematicMotor.CharacterUp).normalized;
	            if (cameraPlanarDirection.sqrMagnitude == 0f)
	            {
	                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, _kinematicMotor.CharacterUp).normalized;
	            }
	            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _kinematicMotor.CharacterUp);

	            // Move and look inputs
	            _moveInputVector = cameraPlanarRotation * moveInputVector;
	            _lookInputVector = cameraPlanarDirection;

	            // Jumping input
	            if (inputs.JumpDown)
	            {
		            _timeSinceJumpRequested = 0f;
		            _jumpRequested = true;
	            }
	        }
		
		public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
		{
			// throw new NotImplementedException();
		}

		public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
		{
			Vector3 addedVelocity = (transform.forward * (_moveInputVector.z * maxStableMoveSpeed) +
			                         transform.right * (_moveInputVector.x * maxStableMoveSpeed)).normalized * deltaTime;
			
			// Apply added velocity
			currentVelocity += addedVelocity;
			
			if (!_kinematicMotor.GroundingStatus.IsStableOnGround)
			{
				// Gravity
				// currentVelocity += gravity * deltaTime;
			}
			
			// Drag
			currentVelocity *= (1f / (1f + (drag * deltaTime)));
			// throw new NotImplementedException();
			
			Debug.Log(_kinematicMotor.GroundingStatus.IsStableOnGround);
		}

		public void BeforeCharacterUpdate(float deltaTime)
		{
			// throw new NotImplementedException();
		}

		public void PostGroundingUpdate(float deltaTime)
		{
			// throw new NotImplementedException();
		}

		public void AfterCharacterUpdate(float deltaTime)
		{
			// throw new NotImplementedException();
		}

		public bool IsColliderValidForCollisions(Collider coll)
		{
			// throw new NotImplementedException();
			return false;
		}

		public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
		{
			// throw new NotImplementedException();
		}

		public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
		{
			// throw new NotImplementedException();
		}

		public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
			Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
		{
			// throw new NotImplementedException();
		}

		public void OnDiscreteCollisionDetected(Collider hitCollider)
		{
			// throw new NotImplementedException();
		}
	}
}
