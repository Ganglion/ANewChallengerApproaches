using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Boomerang : Projectile 
{
	public float degreeRotationPerSecond;
	public float timeBeforeReturn;
	public float boomerangMaxReturnSpeed;
	public float boomerangReturnAcceleration;
	public float forceReturnDistance;
	public float forceReturnMultiplier;
	public bool unmissable;
	public float freeTime;
	public float damageIncrease;
	public float maxDamage;
	public ParticleSystem particleSystem;

	private float originalDamage;

	private GameObject thrower;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(projectileDamage < maxDamage) {
			projectileDamage += damageIncrease * Time.deltaTime;
			if(projectileDamage > maxDamage) {
				projectileDamage = maxDamage;
			}
			var emission = particleSystem.emission;
			emission.rateOverDistanceMultiplier = (projectileDamage - originalDamage)/(maxDamage - projectileDamage);
		}
		transform.Rotate(new Vector3(0, 0, 1), degreeRotationPerSecond * Time.deltaTime);
		if(Vector3.Distance(thrower.transform.position, transform.position) > forceReturnDistance) {
			projectileRigidbody.velocity = (thrower.transform.position -transform.position).normalized * boomerangMaxReturnSpeed * forceReturnMultiplier;
		}
		else {
			if(timeBeforeReturn > 0) {
				timeBeforeReturn -= Time.deltaTime;
			}
			if(timeBeforeReturn <= 0) {
				if(projectileRigidbody.velocity.magnitude <= boomerangMaxReturnSpeed) {
					Vector3 oldVelocity = projectileRigidbody.velocity;
					unitProjectileDirection = (thrower.transform.position - transform.position).normalized;
					oldVelocity.x += unitProjectileDirection.x * Time.deltaTime * boomerangReturnAcceleration;
					oldVelocity.y += unitProjectileDirection.y * Time.deltaTime * boomerangReturnAcceleration;
					projectileRigidbody.velocity = oldVelocity;
					if(unmissable) {
						if(freeTime > 0) {
							freeTime -= Time.deltaTime;
						} else {
							projectileRigidbody.velocity = (thrower.transform.position -transform.position).normalized * boomerangMaxReturnSpeed;
						}
					}
				} else {
					//Set it to move directly towards the player
					projectileRigidbody.velocity = (thrower.transform.position -transform.position).normalized * boomerangMaxReturnSpeed;
				}
			}
		}
	}

	// Sets up projectile properties
	public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, GameObject throwPerson, params Buff[] buffs) {
		projectileDamage = damage;
		originalDamage = damage;
		projectileSpeed = speed;
		projectileLifespan = lifespan;
		projectileBuffs = buffs;
		unitProjectileDirection = direction.normalized;
		projectileRigidbody.velocity = unitProjectileDirection * projectileSpeed;
		thrower = throwPerson;
	}
		
	protected override void UpdateProjectile() {
		// Sets the projectile velocity to move in assigned direction at assigned speed
		//projectileRigidbody.velocity = unitProjectileDirection * projectileSpeed;
	}

	protected override void OnHitEnemy(GameObject hitObject) {
		// Gets the component (of the target hit) that controls the health,
		UnitAttributes targetAttributes = hitObject.GetComponent<UnitAttributes> ();

		// and deal damage to it
		targetAttributes.ApplyAttack (projectileDamage, projectileBuffs);

		// projectile dies
	}

	protected override void OnHitFriendly(GameObject o) {
		if(timeBeforeReturn <= 0) {
			if (o.GetComponent<UseProjectileSkill> () != null) {
				OnProjectileDeath ();
			}
		}
	}


	protected override void OnHitStructure (GameObject hitObject) {
		// If projectile hits a wall, projectile dies
		//OnProjectileDeath ();
	}

	protected override void OnProjectileDeath () {
		thrower.GetComponent<UseProjectileSkill>().maximumTotalAllowedOnScreen++;
		//Detech particle system
		particleSystem.GetComponent<ParticleSystem>().Stop();
		particleSystem.gameObject.transform.parent = null;
		Destroy(particleSystem.gameObject, particleSystem.main.duration);
		// Delete (destroy) the gameobject
		Destroy (this.gameObject);
	}
}