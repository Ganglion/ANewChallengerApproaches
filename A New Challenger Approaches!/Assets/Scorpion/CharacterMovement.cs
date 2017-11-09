using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterMovement : UnitInput {

	public int playerID;
	public Player scorpionPlayer;
	[System.NonSerialized]
	private bool initialized;

	// Components
	protected Animator characterAnimator;

	public int totalJumpsAllowed;
	public float glideRate;

	// Jump variables
	private int totalJumps;
	private bool isGliding;
	private bool upButtonReleased;

	// Used for animations
	protected bool isFacingRight = true;
	public bool IsFacingRight { get { return isFacingRight; } }

	protected override void Awake() {
		base.Awake ();
		characterAnimator = GetComponent<Animator>();
		totalJumps = totalJumpsAllowed;
		isGliding = false;
		upButtonReleased = true;
	}

	protected void Update() {
		if(!ReInput.isReady) return;
		if(!initialized) scorpionPlayer = ReInput.players.GetPlayer(playerID);

		if(scorpionPlayer.GetAxis("Move Vertical") < 0.5f) {
			upButtonReleased = true;
		}
	}
	
	protected void FixedUpdate() {

		// Execute this every frame
		DoPlayerInput();
	}

	protected bool jumpKeyPressed() {
		//return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
		float yScale = scorpionPlayer.GetAxis("Move Vertical");
		return yScale >= 0.5f || scorpionPlayer.GetButtonDown("LT Fire");
	}

	protected void DoPlayerInput() {
		
		// Retrieves values from attributes every frame (as buffs/debuffs may change them)
		float movementSpeed = characterAttributes.CurrentMovementSpeed * characterAttributes.MovementSpeedMultiplier;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
		float jumpHeight = characterAttributes.CurrentJumpHeight;

		bool hasMovedHorizontally = false;

		// Character not on ground?
		if (!characterMovement.collisions.below) {

			// fall
			// Gravity is multiplied by 5 to make the jump less floaty
			if(currentVelocity.y <= 0 && jumpKeyPressed() && !upButtonReleased) {
				isGliding = true;
				currentVelocity.y = Time.deltaTime * glideRate * -1;
			} else {
				currentVelocity.y += Time.deltaTime * -9.81f * 5;
			}
		
		// otherwise,
		} else {

			// don't fall
			currentVelocity.y = 0f;
			totalJumps = totalJumpsAllowed;
			isGliding = false;
		}

		// Pressed left arrow?
		//if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
		if (scorpionPlayer.GetAxis("Move Horizontal") < 0) {
			
			// Accelerate leftwards towards max speed
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
			hasMovedHorizontally = true;
			isFacingRight = false;
		}

		// Pressed right arrow?
		//if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
		if (scorpionPlayer.GetAxis("Move Horizontal") > 0) {

			// Accelerate rightwards towards max speed
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
			hasMovedHorizontally = true;
			isFacingRight = true;
		}

		// Pressed up arrow?
		if (jumpKeyPressed() && upButtonReleased) {
			if(!isGliding) {
				upButtonReleased = false;
			}


			// If character is on ground,
			if (characterMovement.collisions.below) {

				// jump!
				// The equation below calculates the velocity required to reach target jump height.
				// Multiplied by 5 to account for the 5x gravity.
				currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5);
			}
			else if(totalJumps > 0) {
				currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5);
			}
			totalJumps--;
		}

		// Didn't move left or right?
		if (!hasMovedHorizontally) {

			// Decelerate towards 0 speed
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
		}
			
		// All velocity calculated, time to move the player
		// Pressed down button?
		//if (Input.GetKey(KeyCode.DownArrow)) {
		if (scorpionPlayer.GetAxis("Move Vertical") < -0.65f) {
			
			// move player while passing through platforms
			// Use Vector2.down as the second parameter if you want to pass through platforms
			characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.down);

		// otherwise
		} else {
			
			// move player normally
			characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
		}
	}

}
