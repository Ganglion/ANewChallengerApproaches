﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class NekoyuProjectile : MonoBehaviour {

    // Constants
    protected const string ENEMY_LAYER = "Enemy";
    protected const string DESTRUCTIBLE_LAYER = "Destructible";
    protected const string STRUCTURE_LAYER = "Structure";
    protected const string PLAYER_LAYER = "Player";

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
	protected bool isPenetrating;
	protected string projectileName;

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
        MoveProjectile();
    }

    protected virtual void MoveProjectile() {
        //transform.Translate(projectileVelocity * Time.deltaTime);
        //projectileRigidbody.velocity = projectileVelocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        GameObject hitObject = other.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer(ENEMY_LAYER)) {
            OnHitEnemy(hitObject);
        } else if (hitObject.layer == LayerMask.NameToLayer(DESTRUCTIBLE_LAYER)) {
            OnHitDestructible(hitObject);
        } else if (hitObject.layer == LayerMask.NameToLayer(STRUCTURE_LAYER)) {
            OnHitStructure(hitObject);
        } else if (hitObject.layer == LayerMask.NameToLayer(PLAYER_LAYER)) {
            OnHitPlayer(hitObject);
        }
    }

    protected virtual void OnHitEnemy(GameObject hitObject) {
        if (projectileHitEffect != null) {
            Instantiate(projectileHitEffect, transform.position, Quaternion.Euler(Vector3.zero));
        }
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position, projectileBuffs);

		switch (projectileName) {
		case "lifesteal":
			Lifesteal.hitEnemy = true;
			break;
		case "melee":
			MeleeAttack.hitEnemy = true;
			break;
		case "fish":
			SpellSwipe.hitEnemy = true;
			break;
		default:
			break;
		}

		if (!isPenetrating) {
			OnProjectileDeath ();
		}
    }

    protected virtual void OnHitDestructible(GameObject hitObject) {
        OnHitEnemy(hitObject);
    }

    protected virtual void OnHitStructure(GameObject hitObject) {
		if (!isPenetrating) {
			OnProjectileDeath ();
		}
    }

    protected virtual void OnHitPlayer(GameObject hitObject) {
        // Nothing here
    }

    protected virtual void OnProjectileDeath() {
        Destroy(this.gameObject);
    }

}
