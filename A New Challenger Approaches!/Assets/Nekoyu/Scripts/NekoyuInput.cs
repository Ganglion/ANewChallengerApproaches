using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class NekoyuInput : UnitInput {

	public int playerID;
	public Player nekoyuPlayer;
	[System.NonSerialized]
	private bool initialized;

    // Components
    protected Animator characterAnimator;
	private int jumpCount;
	public static bool inAir;
	public static bool attacking = false;
	public static bool isFacingRight = true;
	private bool hasReleasedJump = true;

    protected override void Awake() {
        base.Awake();
        characterAnimator = GetComponent<Animator>();
		isFacingRight = true;
    }

	protected void Update() {//Was FixedUpdate(), but changed to Update() tentatively to fix double-jumping issues.
		if(!ReInput.isReady) return;
		if(!initialized) nekoyuPlayer = ReInput.players.GetPlayer(playerID);
		DoPlayerInput ();
    }

    protected void DoPlayerInput() {
		// Retrieves values from attributes every frame (as buffs/debuffs may change them)
		float movementSpeed = characterAttributes.CurrentMovementSpeed * characterAttributes.MovementSpeedMultiplier;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
		float jumpHeight = characterAttributes.CurrentJumpHeight;
		bool hasMovedHorizontally = false;

		if (!characterMovement.collisions.below) { // Is not on ground?
			currentVelocity.y += Time.deltaTime * -9.81f * 5; // Fall
		} else {
			currentVelocity.y = -0.1f;
		}

		if (!characterMovement.collisions.below) {
			inAir = true;
		} else {
			inAir = false;
		}
		
		if (inAir == true) {
			characterAnimator.SetInteger ("Movement", 2);
		} else {
			characterAnimator.SetInteger ("Movement", 0);
		}

		if (!attacking || inAir) {
			//if (Input.GetKey (KeyCode.LeftArrow)) { // Move left
			if (nekoyuPlayer.GetAxis("Move Horizontal") < 0) {
				currentVelocity.x = Mathf.MoveTowards (currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
				hasMovedHorizontally = true;
				isFacingRight = false;
				if (inAir == false) {	
					characterAnimator.SetInteger ("Movement", 1);
				}
				
			}
			if (nekoyuPlayer.GetAxis("Move Horizontal") > 0) { // Move right
				currentVelocity.x = Mathf.MoveTowards (currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
				hasMovedHorizontally = true;
				isFacingRight = true;
				if (inAir == false) {
					characterAnimator.SetInteger ("Movement", 1);
				}
			}
		}

		if (!inAir) {
			jumpCount = 0;
		}

		if (nekoyuPlayer.GetAxis ("Move Vertical") < .5f) {
			hasReleasedJump = true;
		}

		if (!attacking) {
			if (nekoyuPlayer.GetAxis("Move Vertical") >= .5f && jumpCount == 0 && hasReleasedJump) { // Jump
				hasReleasedJump = false;
				currentVelocity.y = Mathf.Sqrt (jumpHeight * -2f * -9.81f * 5); // Velocity to achieve ideal height
				jumpCount += 1;
			} else if (nekoyuPlayer.GetAxis("Move Vertical") >= .5f && jumpCount == 1 && hasReleasedJump) {
				hasReleasedJump = false;
				currentVelocity.y = Mathf.Sqrt (jumpHeight * -2f * -9.81f * 5); // Velocity to achieve ideal height			
				characterAnimator.Play ("Jumpspin", 0, 0f);
				jumpCount += 1;
			}
		}
		
		if (!hasMovedHorizontally) {
			currentVelocity.x = Mathf.MoveTowards (currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
		}

		// All velocity calculated, move the player accordin2 to velocity
		if (nekoyuPlayer.GetAxis("Move Vertical") < -.65f) {
			// Use Vector2.down as the second parameter if you want to pass through platforms
			characterMovement.Move (currentVelocity * Time.deltaTime, Vector2.down);
		} else {
			characterMovement.Move (currentVelocity * Time.deltaTime, Vector2.zero);
		}
		if (isFacingRight) {
			Vector3 transformScale = transform.localScale;
			transformScale.x = Mathf.Abs (transform.localScale.x);
			transform.localScale = transformScale;
		} else {
			Vector3 transformScale = transform.localScale;
			transformScale.x = -Mathf.Abs (transform.localScale.x);
			transform.localScale = transformScale;
		}
    
	}
	
}

