
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

    protected virtual void FixedUpdate() {
        DoGravity(ref currentVelocity);
        Vector2 nextFrameDisplacement = currentVelocity * Time.deltaTime;
        float angleOfGround = 0;
        Vector2 raycastOrigin = transform.position;
        RaycastHit2D verticalHit = DetectVerticalCollisions(nextFrameDisplacement, ref angleOfGround, ref raycastOrigin);
        RaycastHit2D horizontalHit = DetectHorizontalCollisions(nextFrameDisplacement, raycastOrigin);
        if (nextFrameDisplacement.x != 0) {
            CheckHorizontalCollisions(horizontalHit, angleOfGround, ref currentVelocity, ref nextFrameDisplacement);
        }
        if (Mathf.Sign(nextFrameDisplacement.y) < 0) {
            CheckVerticalCollisions(verticalHit, ref nextFrameDisplacement, ref currentVelocity);
        }
        transform.Translate(nextFrameDisplacement);
	}

    protected void DoGravity(ref Vector2 velocity) {
        velocity.y += gravityAcceleration * Time.deltaTime;
    }

    protected RaycastHit2D DetectHorizontalCollisions(Vector2 displacement, Vector2 raycastOrigin) {
		float directionOfMovementX = System.Math.Sign(currentVelocity.x);
		float directionOfMovementY = Mathf.Sign(displacement.y);
		float raycastOriginY = (directionOfMovementY > 0) ? objectCollider.bounds.max.y : objectCollider.bounds.min.y;
		Vector2 leftRaycastOrigin = new Vector2(objectCollider.bounds.min.x, raycastOriginY);
		Vector2 rightRaycastOrigin = new Vector2(objectCollider.bounds.max.x, raycastOriginY);


		/*
        float directionOfMovementX = Mathf.Sign(displacement.x);
        RaycastHit2D horizontalHit = Physics2D.Raycast(raycastOrigin, Vector2.right * directionOfMovementX, Mathf.Abs(displacement.x), collisionLayerMask);
		Debug.DrawLine(raycastOrigin, raycastOrigin + Vector2.right * directionOfMovementX, Color.green);
		if (isGrounded) {
			RaycastHit2D oppositeHorizontalHit = Physics2D.Raycast(raycastOrigin, Vector2.right * -directionOfMovementX, Mathf.Abs(displacement.x), collisionLayerMask);
		}
        return horizontalHit;*/
    }

    protected RaycastHit2D DetectVerticalCollisions(Vector2 displacement, ref float angleOfGround, ref Vector2 raycastOrigin) {
        float directionOfMovementX = System.Math.Sign(currentVelocity.x);
        float directionOfMovementY = Mathf.Sign(displacement.y);
        float raycastOriginY = (directionOfMovementY > 0) ? objectCollider.bounds.max.y : objectCollider.bounds.min.y;
        Vector2 leftRaycastOrigin = new Vector2(objectCollider.bounds.min.x, raycastOriginY);
        Vector2 rightRaycastOrigin = new Vector2(objectCollider.bounds.max.x, raycastOriginY);
        RaycastHit2D leftVerticalHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);
        RaycastHit2D rightVerticalHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);
        RaycastHit2D verticalHit;
        if (leftVerticalHit.collider == null) {
            raycastOrigin = rightRaycastOrigin;
            verticalHit = rightVerticalHit;
        } else if (rightVerticalHit.collider == null) {
            raycastOrigin = leftRaycastOrigin;
            verticalHit = leftVerticalHit;
        } else {
            raycastOrigin = (leftVerticalHit.distance < rightVerticalHit.distance) ? leftRaycastOrigin : rightRaycastOrigin;
            verticalHit = (leftVerticalHit.distance < rightVerticalHit.distance) ? leftVerticalHit : rightVerticalHit;
        }
        Debug.DrawLine(raycastOrigin, raycastOrigin + Vector2.up * directionOfMovementY, Color.green);
        if (verticalHit.collider != null) {
            angleOfGround = Vector2.SignedAngle(Vector2.up, verticalHit.normal);
        }
        return verticalHit;
    }

	/*if (currentVelocity.x != 0) {
        raycastOriginX = (directionOfMovementX > 0) ? objectCollider.bounds.max.x : objectCollider.bounds.min.x;
    } else {
        raycastOriginX = (angleOfGround > 0) ? objectCollider.bounds.min.x : objectCollider.bounds.max.x;
    }
    Debug.DrawLine(raycastOrigin, raycastOrigin + Vector2.up * directionOfMovementY, Color.green);
    RaycastHit2D verticalHit = Physics2D.Raycast(raycastOrigin, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);
    if (verticalHit.collider != null) {
        angleOfGround = Vector2.SignedAngle(Vector2.up, verticalHit.normal);
    }
    return verticalHit;

    protected Vector2 GetRaycastOriginPosition(ref float angleOfGround) {
        float directionOfMovementX = System.Math.Sign(currentVelocity.x);
        float directionOfMovementY = System.Math.Sign(currentVelocity.y);
        float raycastOriginX;
        if (currentVelocity.x != 0) {
            raycastOriginX = (directionOfMovementX > 0) ? objectCollider.bounds.max.x : objectCollider.bounds.min.x;
        } else {
            raycastOriginX = (angleOfGround > 0) ? objectCollider.bounds.min.x : objectCollider.bounds.max.x;
        }
        float raycastOriginY = (directionOfMovementY > 0) ? objectCollider.bounds.max.y : objectCollider.bounds.min.y;
        return new Vector2(raycastOriginX, raycastOriginY);
    }

    /*protected RaycastHit2D DetectHorizontalCollisions(Vector2 displacement, float heightOfObject, float widthOfObject) {
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
			angleOfGround = Vector2.SignedAngle(Vector2.up, verticalHit.normal);
		} else {
            Debug.Log(verticalHit.collider);
        }
        return verticalHit;
    }*/

    protected void CheckHorizontalCollisions(RaycastHit2D horizontalHit, float angleOfGround, ref Vector2 velocity, ref Vector2 displacement) {
        float directionOfMovementX = Mathf.Sign(displacement.x);
        float idealDisplacementY = Mathf.Sin(angleOfGround * Mathf.Deg2Rad) * Mathf.Abs(displacement.x);
        float idealDisplacementX = Mathf.Cos(angleOfGround * Mathf.Deg2Rad) * Mathf.Abs(displacement.x) * directionOfMovementX;
        if (horizontalHit.collider != null) {
            float obstacleAngle = Vector2.SignedAngle(Vector2.up, horizontalHit.normal);
			if (obstacleAngle > maximumAngleOfSlopeClimbable) {
				displacement.x = (horizontalHit.distance - bufferLength) * directionOfMovementX;
			} else {
				float directionOfDisplacementY = (directionOfMovementX == Mathf.Sign (obstacleAngle)) ? 1 : 0;
				displacement.y = displacement.x * Mathf.Tan(obstacleAngle * Mathf.Deg2Rad) * directionOfDisplacementY;
			}
            isGrounded = true;
        }
        
    }

	protected void CheckVerticalCollisions(RaycastHit2D verticalHit, ref Vector2 displacement, ref Vector2 velocity) {
		float directionOfMovementY = Mathf.Sign(displacement.y);
        /*Vector2 raycastOrigin = (Vector2)transform.position + new Vector2(0, heightOfObject / 2) * directionOfMovementY;
		RaycastHit2D verticalHit = Physics2D.BoxCast(raycastOrigin, new Vector2(widthOfObject, bufferLength), 0, Vector2.up * directionOfMovementY, Mathf.Abs(displacement.y), collisionLayerMask);*/
        if (verticalHit.collider != null) {
            displacement.y = (verticalHit.distance - bufferLength) * directionOfMovementY;
            isGrounded = true;
            velocity.y = 0;
		}
    }
}
