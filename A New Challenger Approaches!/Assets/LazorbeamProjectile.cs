using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazorbeamProjectile : Projectile {

	// Runtime variables
	protected float projectileGravity;

	public void SetupProjectile(float damage, Vector2 velocity, float lifespan, float gravity, GameObject hitEffect) {
		projectileDamage = damage;
		projectileSpeed = velocity.magnitude;
		projectileLifespan = lifespan;
		unitProjectileDirection = velocity.normalized;
		projectileGravity = gravity;
		projectileRigidbody.velocity = velocity;
		projectileHitEffect = hitEffect;
	}

	protected override void UpdateProjectile() {
		projectileRigidbody.velocity += new Vector2(0, projectileGravity * Time.deltaTime);
	}

	protected override void OnHitFriendly (GameObject hitObject) { }

	protected override void OnHitEnemy (GameObject hitObject) {
		CameraController.Instance.ShakeCamera (0.075f, .75f);
		Instantiate (projectileHitEffect, transform.position, Quaternion.Euler (Vector2.zero));
		hitObject.GetComponent<UnitAttributes> ().ApplyAttack (projectileDamage, transform.position, projectileBuffs);
	}

	protected override void OnHitStructure(GameObject hitObject) {
		RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, transform.lossyScale.y, LayerMask.GetMask(STRUCTURE_LAYER));
		Instantiate (projectileHitEffect, groundHit.point, Quaternion.Euler(Vector3.zero));
		float angleOfGround = Vector2.SignedAngle(groundHit.normal, Vector2.up);
		OnProjectileDeath ();
	}

	protected override void OnProjectileDeath () {
		Destroy (this.gameObject);
	}
}
