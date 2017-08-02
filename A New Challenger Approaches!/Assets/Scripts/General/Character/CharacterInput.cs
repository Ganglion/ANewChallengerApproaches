using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMovement))]
public class CharacterInput : MonoBehaviour {

	protected Vector2 currentVelocity;
    protected UnitAttributes characterAttributes;
	protected ObjectMovement characterMovement;

    protected void Awake() {
        characterAttributes = GetComponent<UnitAttributes>();
		characterMovement = GetComponent<ObjectMovement> ();
    }

    protected void FixedUpdate() {
        DoPlayerInput();
    }

    protected void DoPlayerInput() {
        float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
        float jumpHeight = characterAttributes.CurrentJumpHeight;
        bool hasMovedHorizontally = false;

        if (Input.GetKey(KeyCode.A)) {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
        }
        if (Input.GetKey(KeyCode.D)) {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
        }
        if (Input.GetKey(KeyCode.W)) {
			if (characterMovement.collisions.below) {
                currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 2);
            }
        }

        if (!hasMovedHorizontally) {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime);
        }
		currentVelocity.y += Time.deltaTime * -9.81f * 2;
		characterMovement.Move (currentVelocity * Time.deltaTime, Vector2.down);
    }

}
