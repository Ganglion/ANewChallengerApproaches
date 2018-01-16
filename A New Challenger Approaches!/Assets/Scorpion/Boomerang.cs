using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Boomerang : Projectile 
{
	public float degreeRotationPerSecond;
	protected float timeBeforeReturn;
	public float boomerangMaxReturnSpeed;
	public float boomerangAccelerationTime;
	public ParticleSystem particleSystem;

	private float timeSinceReturnStarted;

	private GameObject thrower;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(new Vector3(0, 0, 1), degreeRotationPerSecond * Time.deltaTime);
		if(timeBeforeReturn > 0) {
			timeBeforeReturn -= Time.deltaTime;
		}
		else {
			timeSinceReturnStarted += Time.deltaTime;
			Vector3 oldVelocity = projectileRigidbody.velocity;
			unitProjectileDirection = (thrower.transform.position - transform.position).normalized;
			
			oldVelocity.x = unitProjectileDirection.x * (boomerangMaxReturnSpeed * (timeSinceReturnStarted / boomerangAccelerationTime));
			oldVelocity.y = unitProjectileDirection.y * (boomerangMaxReturnSpeed * (timeSinceReturnStarted / boomerangAccelerationTime));
			
			projectileRigidbody.velocity = oldVelocity;	
		}
	}

	// Sets up projectile properties
	public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, GameObject throwPerson, float returnTime, params Buff[] buffs) {
		projectileDamage = damage;
		projectileSpeed = speed;
		projectileLifespan = lifespan;
		projectileBuffs = buffs;
		timeBeforeReturn = returnTime;
		unitProjectileDirection = direction.normalized;
		projectileRigidbody.velocity = unitProjectileDirection * projectileSpeed;
		thrower = throwPerson;
		timeSinceReturnStarted = 0;
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
			if(o.GetComponent<UseProjectileSkill>() != null) {
				OnProjectileDeath();
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