using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    // Runtime variables
    protected Vector2 velocity;
    protected bool isGrounded = true;
    protected float distanceToBottomOfCharacter;
    protected float widthOfCharacter;

    // Components
    protected Rigidbody2D characterRigidbody;
    protected CharacterAttributes characterAttributes;

    void Awake() {
        characterRigidbody = GetComponent<Rigidbody2D>();
        characterAttributes = GetComponent<CharacterAttributes>();
        distanceToBottomOfCharacter = GetComponent<Collider2D>().bounds.extents.y;
        widthOfCharacter = GetComponent<Collider2D>().bounds.extents.x * 2;
        Debug.Log(distanceToBottomOfCharacter + " " + widthOfCharacter);
    }

	void FixedUpdate () {
        RaycastHit2D groundHit = Physics2D.BoxCast((Vector2)transform.position - new Vector2(0, distanceToBottomOfCharacter), new Vector2(widthOfCharacter, 0.01f), 0, Vector2.down, .01f, LayerMask.GetMask("Structure"));
        if (groundHit.collider != null) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        Debug.Log(isGrounded);

        if (!isGrounded) {
            characterRigidbody.MovePosition((Vector2)transform.position - Time.deltaTime * new Vector2(0, 9.81f));
        }

        if (characterAttributes.CanMove) {
            if (Input.GetKey(KeyCode.A)) {
                characterRigidbody.MovePosition(transform.position + new Vector3(-3, 0) * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.D)) {
                characterRigidbody.MovePosition(transform.position + new Vector3(3, 0) * Time.fixedDeltaTime);
            }
        }
	}
}
