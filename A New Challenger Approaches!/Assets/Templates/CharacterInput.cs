using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : UnitInput {

    // Components
    protected Animator characterAnimator;

    protected override void Awake() {
		base.Awake ();
        characterAnimator = GetComponent<Animator>();
    }

    protected void FixedUpdate() {
        DoPlayerInput();
    }

    protected void DoPlayerInput() {
        // Retrieves values from attributes every frame (as buffs/debuffs may change them)
        float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
        float jumpHeight = characterAttributes.CurrentJumpHeight;
        bool hasMovedHorizontally = false;

        if (!characterMovement.collisions.below) { // Is not on ground?
            currentVelocity.y += Time.deltaTime * -9.81f * 5; // Fall
        } else {
            currentVelocity.y = -0.1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) { // Move left
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
            isFacingRight = false;
        }
        if (Input.GetKey(KeyCode.RightArrow)) { // Move right
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
            isFacingRight = true;
        }
        if (Input.GetKey(KeyCode.UpArrow)) { // Jump
			if (characterMovement.collisions.below) {
                currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5); // Velocity to achieve ideal height
            }
        }

        if (!hasMovedHorizontally) {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
        }

        // All velocity calculated, move the player according to velocity
        if (Input.GetKey(KeyCode.DownArrow)) {
            // Use Vector2.down as the second parameter if you want to pass through platforms
            characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.down);
        } else {
            characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
        }
    }

}
