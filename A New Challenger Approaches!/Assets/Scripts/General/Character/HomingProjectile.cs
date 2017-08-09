using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile {

    // Runtime variables
    protected Transform targetUnit;

    public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, Transform target, params Buff[] buffs) {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = direction.normalized;
        targetUnit = target;
    }

    protected override void MoveProjectile() {
        Vector2 directionTowardsTarget;
        if (targetUnit != null) {
            directionTowardsTarget = (targetUnit.position - transform.position).normalized;
        } else {
            directionTowardsTarget = unitProjectileDirection;
        }
        projectileRigidbody.velocity = directionTowardsTarget * projectileSpeed;
    }

}
