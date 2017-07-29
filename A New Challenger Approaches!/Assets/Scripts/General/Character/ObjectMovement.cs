
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
    protected float gravityAcceleration = -19.62f;

    // Components
    protected Collider2D objectCollider;

    protected virtual void Awake() {
        collisionLayerMask = LayerMask.GetMask("Structure");
        currentVelocity = Vector2.zero;
        objectCollider = GetComponent<Collider2D>();
    }

	protected virtual void FixedUpdate () {
		if (!isGrounded) {
			DoGravity (ref currentVelocity);
		}
        Vector2 nextFrameDisplacement = currentVelocity * Time.deltaTime;
        float heightOfObject = objectCollider.bounds.extents.y * 2;
        float widthOfObject = objectCollider.bounds.extents.x * 2;
        float angleOfGround = 0;
        RaycastHit2D horizontalHit = DetectHorizontalCollisions(nextFrameDisplacement, heightOfObject, widthOfObject);
        RaycastHit2D verticalHit = DetectVerticalCollisions(nextFrameDisplacement, heightOfObject, widthOfObject, ref angleOfGround);
		CheckHorizontalCollisions (horizontalHit, angleOfGround, ref currentVelocity, ref nextFrameDisplacement);
		if (!isGrounded && currentVelocity.y <= 0) {
			CheckVerticalCollisions (verticalHit, ref nextFrameDisplacement, ref currentVelocity);
		}
        transform.Translate(nextFrameDisplacement);
	}

    protected void DoGravity(ref Vector2 velocity) {
        velocity.y += gravityAcceleration * Time.deltaTime;
    }

    protected RaycastHit2D DetectHorizontalCollisions(Vector2 displacement, float heightOfObject, float widthOfObject) {
        float directionOfMovementX = Mathf.Sign(displacement.x);
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(widthOfObject / 2, 0) * directionOfMovementX;
        RaycastHit2D horizontalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(bufferLength, heightOfObject), 0, Vector2.right * directionOfMovementX, Mathf.Abs(displacement.x), collisionLayerMask);
        return horizontalHit;
    }

    protected RaycastHit2D DetectVerticalCollisions(Vector2 displacement, float heightOfObject, float widthOfObject, ref float angleOfGround) {
        float directionOfMovementY = Mathf.Sign(displacement.y);
        Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0, heightOfObject / 2) * directionOfMovementY;
        RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(widthOfObject, bufferLength), 0, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);
		if (verticalHit.collider != null) {
			angleOfGround = Vector2.SignedAngle (verticalHit.normal, Vector2.up);
			Debug.Log(angleOfGround);
		}
        
        return verticalHit;
    }

    protected void CheckHorizontalCollisions(RaycastHit2D horizontalHit, float angleOfGround, ref Vector2 velocity, ref Vector2 displacement) {
        float directionOfMovementX = Mathf.Sign(displacement.x);
        float idealDisplacementY = Mathf.Sin(angleOfGround * Mathf.Deg2Rad) * Mathf.Abs(displacement.x);
        float idealDisplacementX = Mathf.Cos(angleOfGround * Mathf.Deg2Rad) * Mathf.Abs(displacement.x) * directionOfMovementX;
        if (horizontalHit.collider != null) {
            float obstacleAngle = Vector2.Angle(horizontalHit.normal, Vector2.up);
			if (obstacleAngle > maximumAngleOfSlopeClimbable || obstacleAngle != angleOfGround) {
				displacement.x = horizontalHit.distance * directionOfMovementX;
			} else {
				displacement.y = displacement.x * Mathf.Tan (obstacleAngle * Mathf.Deg2Rad);
			}
        }
    }

	protected void CheckVerticalCollisions(RaycastHit2D verticalHit, ref Vector2 displacement, ref Vector2 velocity) {
		float directionOfMovementY = Mathf.Sign(displacement.y);
		/*Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0, heightOfObject / 2) * directionOfMovementY;
		RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(widthOfObject, bufferLength), 0, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);*/
		if (verticalHit.collider != null) {
			displacement.y = verticalHit.distance * directionOfMovementY;
			velocity.y = 0;
			isGrounded = true;
		} else {
			isGrounded = false;
		}
    }
}
