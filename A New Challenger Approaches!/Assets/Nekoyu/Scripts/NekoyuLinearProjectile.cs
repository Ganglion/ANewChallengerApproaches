using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekoyuLinearProjectile : NekoyuProjectile {

	public void SetupProjectile(bool penetration, string name, float damage, float speed, float lifespan, Vector2 direction, params Buff[] buffs) {
		isPenetrating = penetration;
		projectileName = name;
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
