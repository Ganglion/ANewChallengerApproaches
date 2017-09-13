using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringHomingProjectileSkillExample : MonoBehaviour {

    // Constants
    private LayerMask projectileHitMask;

    // Prefabs
    [SerializeField]
    private GameObject projectile;

    // Objects
    [SerializeField]
    private Transform firingIndicator;
    [SerializeField]
    private Transform firingPivot;

    // Fields
    [SerializeField]
    private float projectileDamage;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileLifeSpan;
    [SerializeField]
    private float projectileCooldown;
    [SerializeField]
    private Vector2 detectionBoxSize;
    [SerializeField]
    private bool useColliderAsOffset;
    [SerializeField]
    private float detectionBoxOffset;

    // Runtime variables
    private float currentProjectileCooldown = 0;

    // Components
    private CharacterInput characterMovement;

    private void Awake() {
        characterMovement = GetComponent<CharacterInput>();
        projectileHitMask = LayerMask.GetMask("Enemy", "Destructible");
    }

    private void Update() {
        // Turn on "Gizmos" in game window
        Visualiser.DrawRectangle(CalculateRaycastOriginPosition(), detectionBoxSize, Color.red);

        if (currentProjectileCooldown > 0) {
            currentProjectileCooldown -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.X) && currentProjectileCooldown <= 0) {
            Transform targetTransform = DetectTargetInDetectionBox();
            FireProjectile(targetTransform);
            currentProjectileCooldown = projectileCooldown;
        }
    }

    private Transform DetectTargetInDetectionBox() {
        Vector2 raycastOrigin = CalculateRaycastOriginPosition();
        RaycastHit2D[] hitsInDetectionBox = Physics2D.BoxCastAll(raycastOrigin, detectionBoxSize, 0, Vector2.zero, 0, projectileHitMask);
        Transform nearestTarget = null;
        float minSqrDistance = Mathf.Infinity;
        for (int i = 0; i < hitsInDetectionBox.Length; i++) { // Finds the closest target detected
            float sqrDistanceToTarget = (hitsInDetectionBox[i].point - (Vector2)transform.position).sqrMagnitude;
            if (sqrDistanceToTarget < minSqrDistance) {
                nearestTarget = hitsInDetectionBox[i].transform;
                minSqrDistance = sqrDistanceToTarget;
            }
        }
        
        return nearestTarget;
    }

    private Vector2 CalculateRaycastOriginPosition() {
        int characterOffsetDirection = (characterMovement.IsFacingRight) ? 1 : -1;
        return new Vector2(transform.position.x + (detectionBoxOffset + detectionBoxSize.x / 2) * characterOffsetDirection, transform.position.y);
    }

    private void FireProjectile(Transform targetTransform) {
        GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));
        Vector2 facingVector = firingIndicator.position - firingPivot.position;
        Debug.Assert(newProjectile.GetComponent<ExampleHomingProjectile>(), "Projectile does not contain the HomingProjectile component. Check if you getting the correct component.");
        newProjectile.GetComponent<ExampleHomingProjectile>().SetupProjectile(projectileDamage, projectileSpeed, projectileLifeSpan, facingVector, targetTransform);
    }
}
