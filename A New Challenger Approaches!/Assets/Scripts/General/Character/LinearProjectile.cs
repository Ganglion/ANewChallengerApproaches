using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile {

    public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, params Buff[] buffs) {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = direction.normalized;
    }

    protected override void MoveProjectile() {
        projectileRigidbody.velocity = unitProjectileDirection * projectileSpeed;
    }

}
