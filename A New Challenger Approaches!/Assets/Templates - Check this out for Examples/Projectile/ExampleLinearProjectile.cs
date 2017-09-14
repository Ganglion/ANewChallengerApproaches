using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleLinearProjectile : Projectile {

	// Sets up projectile properties
    public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, params Buff[] buffs) {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = direction.normalized;
    }
		
	protected override void UpdateProjectile() {
		// Sets the projectile velocity to move in assigned direction at assigned speed
        projectileRigidbody.velocity = unitProjectileDirection * projectileSpeed;
    }

	protected override void OnHitEnemy(GameObject hitObject) {
		// Gets the component (of the target hit) that controls the health,
		UnitAttributes targetAttributes = hitObject.GetComponent<UnitAttributes> ();

		// and deal damage to it
		targetAttributes.ApplyAttack (projectileDamage, projectileBuffs);

		// projectile dies
		Destroy (this.gameObject);
	}

	protected override void OnHitStructure (GameObject hitObject) {
		// If projectile hits a wall, projectile dies
		OnProjectileDeath ();
	}

	protected override void OnProjectileDeath () {
		// Delete (destroy) the gameobject
		Destroy (this.gameObject);
	}

}
