using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    Vector2 fireDirection;
    bool isFiring = false;
    float laserUpTime;
	// Use this for initialization
	public void SpawnLaser(float damage, float speed, float shootTime, Vector2 direction)
    {
        projectileDamage = damage;
        fireDirection = direction;
        laserUpTime = shootTime;
        projectileLifespan = 50;

        projectileSpeed = 0;
       
        projectileBuffs = new Buff[0];
        unitProjectileDirection = direction.normalized;
        projectileRigidbody.velocity = Vector2.zero;

    }

    protected override void MoveProjectile() {
        if (isFiring && laserUpTime > 0)
        {
           transform.localScale = new Vector2(0, transform.localScale.y + (1 * Time.deltaTime));
        }

        if (laserUpTime <= 0)
        {
            OnProjectileDeath();
        }
    }

    public void FireLaser()
    {
        isFiring = true;
    }
   

    protected override void OnHitPlayer(GameObject hitObject)
    {
        CameraController.Instance.ShakeCamera(0.075f, .75f);
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position);
    }

    protected override void OnProjectileDeath()
    {
        Debug.Log("LASER Projectile Death");
        Destroy(this.gameObject);
    }

    protected override void OnHitEnemy(GameObject hitObject) { }

     protected override void OnHitStructure(GameObject hitObject)
    { }
}
