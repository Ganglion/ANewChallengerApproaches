
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {

    protected const float bufferLength = 0.005f;
    protected LayerMask collisionLayerMask;
    protected float gravityAcceleration = -19.62f;

    // Runtime variables
    protected Vector2 currentVelocity;

    // Components
    protected Collider2D objectCollider;

    protected virtual void Awake() {
        collisionLayerMask = LayerMask.GetMask("Structure");
        currentVelocity = Vector2.zero;
        objectCollider = GetComponent<Collider2D>();
    }

	protected virtual void FixedUpdate () {
        DoGravity(ref currentVelocity);
        Vector2 nextFrameDisplacement = currentVelocity * Time.deltaTime;
        float heightOfObject = objectCollider.bounds.extents.y * 2;
        float widthOfObject = objectCollider.bounds.extents.x * 2;
        CheckHorizontalCollisions(ref nextFrameDisplacement, heightOfObject, widthOfObject, ref currentVelocity);
        CheckVerticalCollisions(ref nextFrameDisplacement, heightOfObject, widthOfObject, ref currentVelocity);
        transform.Translate(nextFrameDisplacement);
	}

    protected void DoGravity(ref Vector2 velocity) {
        velocity.y += gravityAcceleration * Time.deltaTime;
    }

    protected void CheckHorizontalCollisions(ref Vector2 displacement, float heightOfObject, float widthOfObject, ref Vector2 velocity) {
        float directionOfMovementX = Mathf.Sign(displacement.x);
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(widthOfObject / 2, 0) * directionOfMovementX;
        RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(bufferLength, heightOfObject), 0, Vector2.right * directionOfMovementX, Mathf.Abs(displacement.x), collisionLayerMask);
        if (verticalHit.collider != null) {
            displacement.x = verticalHit.distance * directionOfMovementX;
            velocity.x = 0;
        }
    }

    protected void CheckVerticalCollisions(ref Vector2 displacement, float heightOfObject, float widthOfObject, ref Vector2 velocity) {
        float directionOfMovementY = Mathf.Sign(displacement.y);
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0, heightOfObject / 2) * directionOfMovementY;
        RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(widthOfObject, bufferLength), 0, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);
        if (verticalHit.collider != null) {
            displacement.y = verticalHit.distance * directionOfMovementY;
            velocity.y = 0;
            OnLand();
        }
    }

    protected virtual void OnLand() {
        
    }
}
