using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateProjectile : Projectile {

	// Attach this script to your projectile prefab

	// Sets the properties of the projectile, call this in your attacking scripts. (You can change the method signature if you want; it's not inherited)
	public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, params Buff[] buffs) {
		// Damage the projectile does on hit
		projectileDamage = damage;

		// The magnitude of the projectile's velocity
		projectileSpeed = speed;

		// How long the projectile stays on the map
		projectileLifespan = lifespan;

		// The buffs/debuffs applied to the target on hit
		projectileBuffs = buffs;

		// The initial direction the projectile is fired at
		unitProjectileDirection = direction.normalized;
	}

	// Executes every frame. You can make changes to a projectile's velocity (like gravity), damage (like ramping up over time), etc
	protected override void UpdateProjectile () {
		
	}

	// Executes when the projectile hits an enemy
	protected override void OnHitEnemy (GameObject hitObject) {
		
	}

	// Executes when the projectile hits a friendly player
	protected override void OnHitFriendly (GameObject hitObject) {

	}

	// Executes when the projectile hits a structure
	protected override void OnHitStructure (GameObject hitObject) {

	}

	// Executes when the projectile hits a destructible
	protected override void OnHitDestructible (GameObject hitObject) {

	}
		
	// Executes when the projectile's lifespan is over. (usually used to destroy the projectile)
	protected override void OnProjectileDeath () {
		
	}
		
	// Whenever projectile hits anything
	protected override void OnTriggerEnter2D (Collider2D other) {
		base.OnTriggerEnter2D (other);
	}
}
