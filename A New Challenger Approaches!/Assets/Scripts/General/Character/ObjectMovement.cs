
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {

    protected LayerMask collisionLayerMask;
    protected const float bufferLength = 0.005f;
    protected const float maximumAngleOfSlopeClimbable = 67.5f;

    // Runtime variables
    protected Vector2 currentVelocity;
    protected bool isGrounded;
    protected bool isOnSlope;
    protected float gravityAcceleration = -19.62f;

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
        Debug.Log(isOnSlope);
	}

    protected void DoGravity(ref Vector2 velocity) {
        velocity.y += gravityAcceleration * Time.deltaTime;
    }

    protected void CheckHorizontalCollisions(ref Vector2 displacement, float heightOfObject, float widthOfObject, ref Vector2 velocity) {
        float directionOfMovementX = Mathf.Sign(displacement.x);
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(widthOfObject / 2, 0) * directionOfMovementX;
        RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(bufferLength, heightOfObject), 0, Vector2.right * directionOfMovementX, Mathf.Abs(displacement.x), collisionLayerMask);
        if (verticalHit.collider != null) {
            float obstacleAngle = Vector2.Angle(verticalHit.normal, Vector2.up);
            if (isOnSlope) {
                displacement.y = displacement.x * Mathf.Tan(obstacleAngle * Mathf.Deg2Rad);
            } else {
                displacement.x = verticalHit.distance * directionOfMovementX;
                //velocity.x = 0;
                if (obstacleAngle <= maximumAngleOfSlopeClimbable) {
                    isOnSlope = true;
                } else {
                    isOnSlope = false;
                }
            }
        } else {
            isOnSlope = false;
        }
    }

    protected void CheckVerticalCollisions(ref Vector2 displacement, float heightOfObject, float widthOfObject, ref Vector2 velocity) {
        float directionOfMovementY = Mathf.Sign(displacement.y);
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0, heightOfObject / 2) * directionOfMovementY;
        RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(widthOfObject, bufferLength), 0, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);
        if (verticalHit.collider != null) {
            float obstacleAngle = Vector2.Angle(verticalHit.normal, Vector2.up);
            float directionOfMovementX = Mathf.Sign(displacement.x);
            if (isOnSlope) {
                if (Mathf.Sign(obstacleAngle) == directionOfMovementX) {
                    // Descend slope
                    displacement.y = displacement.x * Mathf.Tan(obstacleAngle * Mathf.Deg2Rad);
                }
            } else {
                displacement.y = verticalHit.distance * directionOfMovementY;
                velocity.y = 0;
            }
        }
    }
}
