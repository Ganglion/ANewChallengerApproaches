using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : Projectile {

	public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, params Buff[] buffs) {
		projectileDamage = damage;
		projectileSpeed = speed;
		projectileLifespan = lifespan;
		projectileBuffs = buffs;
		unitProjectileDirection = direction.normalized;
	}

	protected override void UpdateProjectile () {
		CameraController camera = CameraController.Instance;
		float distanceFromCamera = (transform.position - camera.CameraTransform.position).magnitude;
		CameraController.Instance.ShakeCamera (.033f, 1f);
		projectileRigidbody.velocity = unitProjectileDirection * projectileSpeed;
	}

	protected override void OnHitFriendly (GameObject hitObject) { }

	protected override void OnHitEnemy (GameObject hitObject) {
		hitObject.GetComponent<UnitAttributes> ().ApplyAttack (projectileDamage, transform.position);
	}

	protected override void OnHitStructure (GameObject hitObject) { }

}
