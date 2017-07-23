using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : ObjectMovement {

    protected bool isGrounded = true;

    protected UnitAttributes characterAttributes;

    protected override void Awake() {
        base.Awake();
        characterAttributes = GetComponent<UnitAttributes>();
    }

    protected override void FixedUpdate() {
        DoPlayerInput();
        base.FixedUpdate();
    }

    protected void DoPlayerInput() {
        float movementSpeed = characterAttributes.CurrentMovementSpeed;
        float currentAcceleration = isGrounded ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
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
            if (isGrounded) {
                currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
                isGrounded = false;
            }
        }

        if (!hasMovedHorizontally) {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime);
        }
    }

    protected override void OnLand() {
        isGrounded = true;
    }
}
