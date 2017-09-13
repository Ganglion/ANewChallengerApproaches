using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateProjectile : Projectile {

	// Attach this script to your projectile prefab

	/// <summary>
	/// Sets the properties of the projectile, call this in your attacking scripts. (You can change the method signature if you want; it's not inherited)
	/// </summary>
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

	/// <summary>
	/// Executes every frame. You can make changes to a projectile's velocity (like gravity), damage (like ramping up over time), etc
	/// </summary>
	protected override void UpdateProjectile () {
		
	}

	/// <summary>
	/// Executes when the projectile hits an enemy
	/// </summary>
	protected override void OnHitEnemy (GameObject hitObject) {
		
	}

	/// <summary>
	/// Executes when the projectile hits a friendly player
	/// </summary>
	protected override void OnHitFriendly (GameObject hitObject) {

	}

	/// <summary>
	/// Executes when the projectile hits a structure
	/// </summary>
	/// <param name="hitObject">Hit object.</param>
	protected override void OnHitStructure (GameObject hitObject) {

	}

	/// <summary>
	/// Executes when the projectile hits a destructible
	/// </summary>
	protected override void OnHitDestructible (GameObject hitObject) {

	}

	/// <summary>
	/// Executes when the projectile's lifespan is over. (usually used to destroy the projectile)
	/// </summary>
	protected override void OnProjectileDeath () {
		
	}

}
