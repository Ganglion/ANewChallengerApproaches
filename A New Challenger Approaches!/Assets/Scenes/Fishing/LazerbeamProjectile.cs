using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerbeamProjectile : Projectile {

    public void SetupProjectile(float damage, Vector2 velocity, float lifespan, GameObject hitEffect) {
        projectileDamage = damage;
        projectileSpeed = velocity.magnitude;
        projectileLifespan = lifespan;
        unitProjectileDirection = velocity.normalized;
        projectileRigidbody.velocity = velocity;
        projectileHitEffect = hitEffect;
    }

    protected override void OnHitEnemy(GameObject hitObject) {
        if (projectileHitEffect != null) {
            Instantiate(projectileHitEffect, transform.position, Quaternion.Euler(Vector3.zero));
        }
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position, projectileBuffs);
    }

	protected override void OnHitStructure (GameObject hitObject) {
		
	}

    protected override void OnProjectileDeath() {
        Destroy(this.gameObject);
    }

}
