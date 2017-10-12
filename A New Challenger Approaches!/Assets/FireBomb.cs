using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : Projectile {
    protected float projectileGravity;
    protected float projectileAcceleration;
    protected int maxHitCount = 6;
    protected bool hasFired = false;
    protected Vector2 velocityOnFire;
    public void SetupProjectile(float damage, Vector2 velocityOnFire, float lifespan, float gravity, float acceleration, params Buff[] buffs)
    {
        projectileDamage = damage;
        projectileSpeed = velocityOnFire.magnitude;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = velocityOnFire.normalized;
        projectileGravity = gravity;
        projectileRigidbody.velocity = new Vector2(0,0);
        projectileAcceleration = acceleration;
        this.velocityOnFire = velocityOnFire;
       
    }


    public void FireProjectile()
    {
        hasFired = true;
    }

    protected void MoveProjectile()
    {
        if (hasFired) {
            projectileRigidbody.velocity += projectileAcceleration * velocityOnFire * Time.deltaTime;
        } 
    }

    

    protected override void OnProjectileDeath()
    {
        Debug.Log("Projectile Death");
        Destroy(this.gameObject);
    }

    protected void OnHitPlayer(GameObject hitObject)
    {
        CameraController.Instance.ShakeCamera(0.075f, .75f);
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position);
        OnProjectileDeath();
    }

    protected override void OnHitEnemy(GameObject hitObject) { }

}
