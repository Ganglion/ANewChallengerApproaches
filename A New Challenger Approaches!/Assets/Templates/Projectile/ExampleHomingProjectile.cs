using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleHomingProjectile : Projectile {

    // Runtime variables
    protected Transform targetUnit;

    public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, Transform target, params Buff[] buffs) {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = direction.normalized;

		// Extra target unit variable
        targetUnit = target;
    }

    protected override void UpdateProjectile() {
        Vector2 directionTowardsTarget;

		// If have target
        if (targetUnit != null) {

			// move towards target
            directionTowardsTarget = (targetUnit.position - transform.position).normalized;

        } else {

			// otherwise, move like a linear projectile
            directionTowardsTarget = unitProjectileDirection;
        }

		// Set projectile velocity according to above calculated direction
        projectileRigidbody.velocity = directionTowardsTarget * projectileSpeed;
    }

}
