using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile {
    protected float projectileGravity;
    protected bool isGoingDown = false;
    protected bool projectileStop = false;
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

    protected override void MoveProjectile()
    {
        //check for going down;
        if(projectileRigidbody.velocity.y <= 0)
        {
            isGoingDown = true;
        }

       
            projectileRigidbody.velocity += new Vector2(0, projectileGravity * Time.deltaTime);


    }

    protected override void OnProjectileDeath()
    {
        Debug.Log("Projectile Death");
        Destroy(this.gameObject);
    }

    protected override void OnHitPlayer(GameObject hitObject)
    {
        CameraController.Instance.ShakeCamera(0.075f, .75f);
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position);
        OnProjectileDeath();
    }

    

    protected override void OnHitEnemy(GameObject hitObject) { }

    protected override void OnHitStructure(GameObject hitObject)
    {
        if (!isGoingDown)
        {
            return;

        }
        if(hitObject.tag == "Through")       {
            int rand = Random.Range(0, 10);
            if (rand < 7)
            {
                return;
            }
            
        }

        OnProjectileStop();
        
    }

    protected void OnProjectileStop()
    {
        projectileRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
    }

}
