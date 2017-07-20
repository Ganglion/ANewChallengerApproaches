using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {

    // Runtime variables
    protected Vector2 velocity;
    protected bool isGrounded = true;
    protected float distanceToBottomOfCharacter;
    protected float widthOfCharacter;

    // Components
    protected Rigidbody2D unitRigidbody;
    protected UnitAttributes unitAttributes;

    void Awake() {
		unitRigidbody = GetComponent<Rigidbody2D>();
		unitAttributes = GetComponent<UnitAttributes>();
        distanceToBottomOfCharacter = GetComponent<Collider2D>().bounds.extents.y;
        widthOfCharacter = GetComponent<Collider2D>().bounds.extents.x * 2;
    }

	void FixedUpdate () {
        RaycastHit2D groundHit = Physics2D.BoxCast((Vector2)transform.position - new Vector2(0, distanceToBottomOfCharacter), new Vector2(widthOfCharacter, 0.5f), 0, Vector2.down, .01f, LayerMask.GetMask("Structure"));
		if (groundHit.collider != null && velocity.y <= 0) {
            isGrounded = true;
			velocity.y = 0;
		} else {
            isGrounded = false;
        }
		Debug.Log (isGrounded);
		Vector2 groundMovementVector = new Vector2(groundHit.normal.y, -groundHit.normal.x);
		Vector2 currentVelocity = velocity;

        

		bool isMovingHorizontally = false;
		if (unitAttributes.CanMove) {
            if (Input.GetKey(KeyCode.A)) {
				if (isGrounded) {
					currentVelocity = Vector2.MoveTowards (currentVelocity, -3f * groundMovementVector, 18f * Time.deltaTime);
				} else {
					currentVelocity.x = Mathf.MoveTowards (currentVelocity.x, -3f, 6f * Time.deltaTime);
				}
				isMovingHorizontally = true;
            }
            if (Input.GetKey(KeyCode.D)) {
				if (isGrounded) {
					currentVelocity = Vector2.MoveTowards (currentVelocity, 3f * groundMovementVector, 18f * Time.deltaTime);
				} else {
					currentVelocity.x = Mathf.MoveTowards (currentVelocity.x, 3f, 6f * Time.deltaTime);
				}
				isMovingHorizontally = true;
            }
			if (Input.GetKey (KeyCode.W) && isGrounded) {
				currentVelocity.y = 0;
				currentVelocity += Vector2.up * 6.25f;
			}
        }

		if (!isMovingHorizontally) {
			currentVelocity.x = Mathf.MoveTowards (currentVelocity.x, 0, 18f * Time.deltaTime);
		}
		if (!isGrounded) {
			currentVelocity.y -= 9.81f * 2 * Time.deltaTime;
		}

		velocity = currentVelocity;
		unitRigidbody.MovePosition ((Vector2)transform.position + currentVelocity * Time.deltaTime);
	}
}
