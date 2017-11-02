using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile {
    protected float projectileGravity;
    protected int hitCount = 0;
    protected int maxHitCount = 6;
    public void SetupProjectile(float damage, Vector2 velocity, float lifespan, float gravity, params Buff[] buffs)
    {
        projectileDamage = damage;
        projectileSpeed = velocity.magnitude;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = velocity.normalized;
        projectileGravity = gravity;
        projectileRigidbody.velocity = velocity;
    }

    protected override void OnProjectileDeath()
    {
        Destroy(this.gameObject);
    }


    protected override void OnHitEnemy(GameObject hitObject) {
        CameraController.Instance.ShakeCamera(0.075f, .75f);
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position);
        OnProjectileDeath();
    }

    protected override void OnHitStructure(GameObject hitObject)
    {
        hitCount++;
        if(hitCount > maxHitCount)
        {
            OnProjectileDeath();
        }
        //changeProjectileTrajectory
        Vector2 wallNormal = hitObject.transform.up;
        projectileRigidbody.velocity=  Vector2.Reflect(projectileRigidbody.velocity, wallNormal);
        
    }


}
