using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCharacterMovement : UnitInput {

    // Components
    protected Animator characterAnimator;

	// Used for animations
	protected bool isFacingRight = true;
	public bool IsFacingRight { get { return isFacingRight; } }

    protected override void Awake() {
		base.Awake ();
        characterAnimator = GetComponent<Animator>();
    }
		
    protected void FixedUpdate() {

		// Execute this every frame
        DoPlayerInput();
    }

    protected void DoPlayerInput() {
        
		// Retrieves values from attributes every frame (as buffs/debuffs may change them)
        float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
        float jumpHeight = characterAttributes.CurrentJumpHeight;

        bool hasMovedHorizontally = false;

		// Character not on ground?
        if (!characterMovement.collisions.below) {

			// fall
			// Gravity is multiplied by 5 to make the jump less floaty
            currentVelocity.y += Time.deltaTime * -9.81f * 5;
		
		// otherwise,
        } else {

			// don't fall
            currentVelocity.y = 0f;
        }

		// Pressed left arrow?
        if (Input.GetKey(KeyCode.LeftArrow)) {

			// Accelerate leftwards towards max speed
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
            isFacingRight = false;
        }

		// Pressed right arrow?
        if (Input.GetKey(KeyCode.RightArrow)) {

			// Accelerate rightwards towards max speed
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
            isFacingRight = true;
        }

		// Pressed up arrow?
        if (Input.GetKey(KeyCode.UpArrow)) {

			// If character is on ground,
			if (characterMovement.collisions.below) {

				// jump!
				// The equation below calculates the velocity required to reach target jump height.
				// Multiplied by 5 to account for the 5x gravity.
				currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5);
            }
        }

		// Didn't move left or right?
        if (!hasMovedHorizontally) {

			// Decelerate towards 0 speed
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
        }
			
        // All velocity calculated, time to move the player
		// Pressed down button?
        if (Input.GetKey(KeyCode.DownArrow)) {

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
