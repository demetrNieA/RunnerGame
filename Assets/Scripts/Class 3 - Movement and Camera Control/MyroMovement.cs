using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirectionEnum
{
    Forward, Back, Left, Right,
    ForwardLeft, ForwardRight, BackLeft, BackRight,
    Idle
}

public enum MyroMoveState { NormalMovement, MidAir, Gliding, Dead }

[RequireComponent(typeof(CharacterController))]
public class MyroMovement : MonoBehaviour
{
	[SerializeField] MyroAnimator myroAnimator;
	[SerializeField] MyroController myroController;
	MyroControlVariant movementControlVariant;

	public float moveSpeed = 10.0f;
	public float rotationSpeed = 140.0f;
	public float jumpPower = 5.0f;
	public float gravity = 20.0f;
	public float glideFallRate = 150.0f;

	
	MyroMoveState myroMoveState = MyroMoveState.NormalMovement;

	// These variables are related to under-the-hood movement controls
	bool jumped = false;
	Vector3 moveDirection = Vector3.zero;
	CharacterController controller;
	float jumpHelperFloatTimer = 0.0f;
	float jumpHelperFloatTime = .25f;

	// Manipulated by the CameraController to communicate to this script that the camera is in first person mode
	MoveDirectionEnum moveDirectionEnum = MoveDirectionEnum.Forward;

	

	// Start is called before the first frame update
	void Start()
	{
		// Find the attached(and required) CharacterController component
		controller = GetComponent<CharacterController>();

		
	}

	// Update is called once per frame
	void Update()
	{
		if (movementControlVariant != null)
		{
			movementControlVariant.RunMovementUpdateVariant(this);
		}
		else
		{
			switch (myroMoveState)
			{
				case MyroMoveState.NormalMovement:
					RunNormalMovementUpdate();
					break;
				case MyroMoveState.MidAir:
					RunMidAirUpdate();
					break;
				case MyroMoveState.Gliding:
					RunGlidingUpdate();
					break;
				case MyroMoveState.Dead:

					break;
			}
		}
	}

	// Runs every physics frame
	void FixedUpdate()
	{
		if (movementControlVariant != null)
		{
			movementControlVariant.RunMovementFixedUpdateVariant(this);
		}
		else
		{
			switch (myroMoveState)
			{
				case MyroMoveState.NormalMovement:
					RunNormalMovementFixedUpdate();
					break;
				case MyroMoveState.MidAir:
					RunMidAirFixedUpdate();
					break;
				case MyroMoveState.Gliding:
					RunGlidingFixedUpdate();
					break;
				case MyroMoveState.Dead:

					break;
			}
		}
	}

	#region NormalMovement
	public void RunNormalMovementUpdate()
    {
		// Jump is in Update() because we use GetKeyDown().
		//  -Sometimes FixedUpdate() misses GetKeyDown(). 
		if (controller.isGrounded || jumpHelperFloatTimer < jumpHelperFloatTime)
		{
			// This counts up in Update and is reset if grounded during FixedUpdate
			jumpHelperFloatTimer += Time.deltaTime;
			// Read the Input Key for Jumping
			if (myroController.JumpKeyIsPressedDown())
				jumped = true;
		}
	}
	public void RunNormalMovementFixedUpdate(float moveSpeedModifier = 1)
    {
		

		myroAnimator.SetGrounded(controller.isGrounded);
		//CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded)
		{
			
			// The keyboard presses will be converted into a Vector3 indicating direction, and multiplied by speed in GetMovementVectorBasedOnKeys()
			moveDirection = GetMovementVectorBasedOnKeys();
			// Convert the movement vector to relate to the player's orientation
			moveDirection = transform.TransformDirection(moveDirection);

			// Modify how far the player moves this frame
			moveDirection *= moveSpeedModifier;


			// This is used to give some grace when jumping after grounded is disabled.
			jumpHelperFloatTimer = 0;
			
		}
		// Jump was flagged in Update(), but is executed here in FixedUpdate()
		if (jumped)
		{
			moveDirection.y += (jumpPower);
			// Jump will be possible again, once the player is grounded.
			jumped = false;
			myroAnimator.JumpTrigger();
			myroMoveState = MyroMoveState.MidAir;
		}

		RotateFromKeyboardPresses();

		// This is pseudo-gravity to avoid the need for a Rigidbody component
		moveDirection.y -= gravity * Time.fixedDeltaTime;
		// Move the CharacterController with math, utilizing the magic of the engine's algorithms to keep you out of other collidable objects
		controller.Move(moveDirection * Time.fixedDeltaTime);
	}

    #endregion

    #region MidAir
	void RunMidAirUpdate()
    {
        if (myroController.JumpKeyIsPressedDown())
        {
			TriggerGlide();
        }
    }

	void RunMidAirFixedUpdate()
	{
		ApplyGravity();

		RotateFromKeyboardPresses();

		if (controller.isGrounded)
		{
			myroMoveState = MyroMoveState.NormalMovement;

		}
	}

	public void ApplyGravity()
    {
		myroAnimator.SetGrounded(controller.isGrounded);
		// This is pseudo-gravity to avoid the need for a Rigidbody component
		moveDirection.y -= gravity * Time.fixedDeltaTime;
		// Move the CharacterController with math, utilizing the magic of the engine's algorithms to keep you out of other collidable objects
		controller.Move(moveDirection * Time.fixedDeltaTime);
	}
    #endregion

    #region Gliding
	void TriggerGlide()
    {
		myroMoveState = MyroMoveState.Gliding;
		myroAnimator.SetGliding(true);
		moveDirection.y = -Time.fixedDeltaTime * glideFallRate;
		controller.height = .5f;
    }

	void RunGlidingUpdate()
    {
        if (myroController.JumpKeyIsPressedDown()) DropGlide();
    }

	void DropGlide()
    {
		myroMoveState = MyroMoveState.NormalMovement;
		myroAnimator.SetGliding(false);
		controller.height = 2;
	}
	void RunGlidingFixedUpdate()
    {
		myroAnimator.SetGrounded(controller.isGrounded);
		// This is pseudo-gravity to avoid the need for a Rigidbody component
		// Move the CharacterController with math, utilizing the magic of the engine's algorithms to keep you out of other collidable objects
		moveDirection = transform.forward * moveSpeed;
		moveDirection.y = -Time.fixedDeltaTime * glideFallRate;
		controller.Move(moveDirection * Time.fixedDeltaTime);

		RotateFromKeyboardPresses();
		if (controller.isGrounded)
		{
			DropGlide();
		}
	}
	#endregion

	#region Death and Revive
	public void SetDead()
	{
		myroMoveState = MyroMoveState.Dead;
	}
	public void Revive()
	{
		myroMoveState = MyroMoveState.NormalMovement;
	}
	#endregion

	#region Utility
	public void SetToNormalMovement()
    {
		myroMoveState = MyroMoveState.NormalMovement;
	}
	public void DisableMovementControls()
    {
		myroMoveState = MyroMoveState.Dead;
	}
	private Vector3 GetMovementVectorBasedOnKeys()
	{
		Vector3 translation = new Vector3();

		if (myroController.ForwardMovementKeyPressed())
			translation += Vector3.forward;
		if (myroController.BackwardMovementKeyPressed())
			translation += Vector3.back;
		if (myroController.StrafeRightMovementKeyPressed())
			translation += Vector3.right;
		if (myroController.StrafeLeftMovementKeyPressed())
			translation += Vector3.left;

		// Normalizes (sets the magnitude of the vector to 1) then multiplies by the moveSpeed
		translation = translation.normalized * moveSpeed;

		// If the movement isn't naught, flag the model to animate walking
		myroAnimator.SetWalking(translation != Vector3.zero);

		// These determine the move direction in one of 9 ways, then orients the fighter model accordingly
		SetMoveDirectionEnumBasedOnVector(translation);
		

		return translation;
	}

	void SetMoveDirectionEnumBasedOnVector(Vector3 translation)
	{
		// Strafe Left, Strafe Right, or Idle
		if (translation.z == 0)
		{
			// Idle
			if (translation.x == 0)
			{
				moveDirectionEnum = MoveDirectionEnum.Idle;
			}
			if (translation.x > 0)
			{
				moveDirectionEnum = MoveDirectionEnum.Right;
			}
			if (translation.x < 0)
			{
				moveDirectionEnum = MoveDirectionEnum.Left;
			}
		}
		else
		{
			// Going Forward
			if (translation.z > 0)
			{
				// Idle
				if (translation.x == 0)
				{
					moveDirectionEnum = MoveDirectionEnum.Forward;
				}
				if (translation.x > 0)
				{
					moveDirectionEnum = MoveDirectionEnum.ForwardRight;
				}
				if (translation.x < 0)
				{
					moveDirectionEnum = MoveDirectionEnum.ForwardLeft;
				}
			}
			else
			{
				// Going Backward
				if (translation.z < 0)
				{
					// Idle
					if (translation.x == 0)
					{
						moveDirectionEnum = MoveDirectionEnum.Back;
					}
					if (translation.x > 0)
					{
						moveDirectionEnum = MoveDirectionEnum.BackRight;
					}
					if (translation.x < 0)
					{
						moveDirectionEnum = MoveDirectionEnum.BackLeft;
					}
				}
			}
		}
	}

	void RotateFromKeyboardPresses()
    {
		float rotation = 0;
		if (Input.GetKey(KeyCode.A) && Input.GetMouseButton(1) == false)
			rotation -= 1;
		if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(1) == false)
			rotation += 1;

		rotation = rotation * Time.fixedDeltaTime * rotationSpeed;
		transform.Rotate(0, rotation, 0);
	}
    #endregion

    #region Control Variant Functions
	public CharacterController GetCharacterController()
    {
		return controller;
    }
	public MyroMoveState GetMoveState()
    {
		return myroMoveState;
    }
	public MyroController GetMyroController()
    {
		return myroController;
    }
	public MyroAnimator GetMyroAnimator()
    {
		return myroAnimator;
    }

	public void SetControlVariant(MyroControlVariant variant)
    {
		movementControlVariant = variant;
    }
	public void EndControlVariant()
    {
		movementControlVariant = null;
    }
	
    #endregion
}
