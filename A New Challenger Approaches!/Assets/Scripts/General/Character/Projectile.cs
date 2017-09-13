using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

    // Constants
    protected const string ENEMY_LAYER = "Enemy";
    protected const string DESTRUCTIBLE_LAYER = "Destructible";
    protected const string STRUCTURE_LAYER = "Structure";
    protected const string PLAYER_LAYER = "Player";

	protected const string PLAYER_TAG = "Player";
	protected const string ENEMY_TAG = "Enemy";

    // Prefabs
    [SerializeField]
    protected GameObject projectileHitEffect;

    // Fields
    [SerializeField]
    protected bool faceTowardsMovement;
    [SerializeField]
    protected float facingDirectionOffset;

    // Attributes
    protected float projectileDamage;
    protected float projectileSpeed;
    protected float projectileLifespan;
    protected Buff[] projectileBuffs;
    protected Vector2 unitProjectileDirection;

    // Runtime variables
    protected float hitColliderRadius;

    // Components
    protected Rigidbody2D projectileRigidbody;

    protected void Awake() {
        projectileRigidbody = GetComponent<Rigidbody2D>();
    }

    protected void Update() {
        if (projectileLifespan > 0) {
            projectileLifespan -= Time.deltaTime;
        } else {
            OnProjectileDeath();
        }
    }

    protected void FixedUpdate() {
        if (faceTowardsMovement) {
            float facingAngle = Mathf.Atan2(projectileRigidbody.velocity.y, projectileRigidbody.velocity.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, facingAngle + facingDirectionOffset);
        }
        UpdateProjectile();
    }

    protected virtual void UpdateProjectile() {
        //transform.Translate(projectileVelocity * Time.deltaTime);
        //projectileRigidbody.velocity = projectileVelocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        GameObject hitObject = other.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer(ENEMY_LAYER)) {
			if (this.gameObject.tag == PLAYER_TAG) {
				OnHitEnemy (hitObject);
			} else if (this.gameObject.tag == ENEMY_TAG) {
				OnHitFriendly (hitObject);
			}
        } else if (hitObject.layer == LayerMask.NameToLayer(DESTRUCTIBLE_LAYER)) {
            OnHitDestructible(hitObject);
        } else if (hitObject.layer == LayerMask.NameToLayer(STRUCTURE_LAYER)) {
            OnHitStructure(hitObject);
		} else if (hitObject.layer == LayerMask.NameToLayer(PLAYER_LAYER)) {
			if (this.gameObject.tag == PLAYER_TAG) {
				OnHitFriendly (hitObject);
			} else if (this.gameObject.tag == ENEMY_TAG) {
				OnHitEnemy (hitObject);
			}
        }
    }

    protected virtual void OnHitEnemy(GameObject hitObject) {
        if (projectileHitEffect != null) {
            Instantiate(projectileHitEffect, transform.position, Quaternion.Euler(Vector3.zero));
        }
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position, projectileBuffs);
        OnProjectileDeath();
    }

    protected virtual void OnHitDestructible(GameObject hitObject) {
        OnHitEnemy(hitObject);
    }

    protected virtual void OnHitStructure(GameObject hitObject) {
        OnProjectileDeath();
    }

    protected virtual void OnHitFriendly(GameObject hitObject) {
		// Nothing here
    }

    protected virtual void OnProjectileDeath() {
        Destroy(this.gameObject);
    }

}
