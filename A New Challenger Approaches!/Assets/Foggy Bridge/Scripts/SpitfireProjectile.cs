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

    protected override void MoveProjectile() {
        projectileRigidbody.velocity += new Vector2(0, projectileGravity * Time.deltaTime);
    }

    protected override void OnHitStructure(GameObject hitObject) {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, projectileRigidbody.velocity.y, LayerMask.NameToLayer(STRUCTURE_LAYER));
        float angleOfGround = Vector2.SignedAngle(groundHit.normal, Vector2.up);
        GameObject newSpitfireFlames = (GameObject)Instantiate(spitfireFlames, groundHit.point, Quaternion.Euler(new Vector3(0, 0, angleOfGround)));
        base.OnHitDestructible(hitObject);
    }
}
