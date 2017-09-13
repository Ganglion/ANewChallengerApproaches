using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireProjectile : Projectile {

    // Prefabs
    [SerializeField]
    protected GameObject spitfireFlames;

    // Runtime variables
    protected float projectileGravity;

    public void SetupProjectile(float damage, Vector2 velocity, float lifespan, float gravity, params Buff[] buffs) {
        projectileDamage = damage;
        projectileSpeed = velocity.magnitude;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = velocity.normalized;
        projectileGravity = gravity;
        projectileRigidbody.velocity = velocity;
    }

    protected override void UpdateProjectile() {
        projectileRigidbody.velocity += new Vector2(0, projectileGravity * Time.deltaTime);
    }

	protected override void OnHitFriendly (GameObject hitObject) { }

	protected override void OnHitEnemy (GameObject hitObject) {
		CameraController.Instance.ShakeCamera (0.075f, .75f);
		hitObject.GetComponent<UnitAttributes> ().ApplyAttack (projectileDamage, transform.position, projectileBuffs);
		OnProjectileDeath ();
	}

    protected override void OnHitStructure(GameObject hitObject) {
		CameraController.Instance.ShakeCamera (0.075f, .75f);
		RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, transform.lossyScale.y, LayerMask.GetMask(STRUCTURE_LAYER));
		Instantiate (projectileHitEffect, groundHit.point, Quaternion.Euler(Vector3.zero));
        float angleOfGround = Vector2.SignedAngle(groundHit.normal, Vector2.up);
        GameObject newSpitfireFlames = (GameObject)Instantiate(spitfireFlames, groundHit.point, Quaternion.Euler(new Vector3(0, 0, angleOfGround)));
        newSpitfireFlames.GetComponent<SpitfireFlamesDamage>().InitialiseSpitfireFlames(projectileBuffs);
		OnProjectileDeath ();
    }

	protected override void OnProjectileDeath () {
		Destroy (this.gameObject);
	}

}
