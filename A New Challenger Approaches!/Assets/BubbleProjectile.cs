using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : Projectile {

    public void SetupProjectile(float damage, Vector2 velocity, float lifespan) {
        projectileDamage = damage;
        projectileSpeed = velocity.magnitude;
        projectileLifespan = lifespan;
        unitProjectileDirection = velocity.normalized;
        projectileRigidbody.velocity = velocity;
    }

    protected override void OnHitEnemy(GameObject hitObject) {

    }

    protected override void OnHitStructure(GameObject hitObject) {
        
    }

    protected virtual void OnTriggerStay2D(Collider2D other) {
        GameObject hitObject = other.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer(PLAYER_LAYER)) {
            if (this.gameObject.tag == ENEMY_TAG) {
                hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage * Time.deltaTime, transform.position, projectileBuffs);
            }
        }
    }

    protected override void OnProjectileDeath() {
        Debug.Log(projectileLifespan);
        Destroy(this.gameObject);
    }
}
