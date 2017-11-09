using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UseProjectileSkill : MonoBehaviour {

	public int playerID;
	public Player scorpionPlayer;
	[System.NonSerialized]
	private bool initialized;

	// Prefabs
	[SerializeField]
	private GameObject projectile;

	// Objects
	[SerializeField]
	private Transform firingIndicator;
	[SerializeField]
	private Transform firingPivot;

	// Fields
	[SerializeField]
	private float projectileDamage;
	[SerializeField]
	private float projectileSpeed;
	[SerializeField]
	private float projectileLifeSpan;
	[SerializeField]
	private float projectileCooldown;

	private UnitAttributes scorpionAttributes;
	private MovementSpeedBuff projectileBuff;
	public int maximumTotalAllowedOnScreen;

	private void Awake() {
		scorpionAttributes = GetComponent<UnitAttributes> ();
		// Uncomment this (and put projectileBuff as an argument in SetupProjectile()) to apply a slow buff to the example character's projectiles
		//projectileBuff = new MovementSpeedBuff (.75f, "EXAMPLE_SLOW", 3, null, false);
	}

	// Runtime variables
	private float currentProjectileCooldown = 0;

	// Executes every frame
	private void Update () {
		if(!ReInput.isReady) return;
		if(!initialized) scorpionPlayer = ReInput.players.GetPlayer(playerID);

		// Skill is on cooldown?
		if (currentProjectileCooldown > 0) {

			// Decrease cooldown time according to frame time
			currentProjectileCooldown -= Time.deltaTime;
		}

		// Pressed C button and skill is off cooldown?
		//if (Input.GetMouseButtonDown(0) && currentProjectileCooldown <= 0) {
		Debug.Log(new Vector2(scorpionPlayer.GetAxis("RS Horizontal"), scorpionPlayer.GetAxis("RS Vertical")));
		if (scorpionPlayer.GetButtonDown("RT Fire") && currentProjectileCooldown <= 0) {

			FireProjectile(new Vector2(scorpionPlayer.GetAxis("RS Horizontal"), scorpionPlayer.GetAxis("RS Vertical")));

			// Set cooldown for the skill
			currentProjectileCooldown = projectileCooldown;
		}

		/*
		// Pressed C button and skill is off cooldown?
        else if (Input.GetKey(KeyCode.C) && currentProjectileCooldown <= 0) {

			// Fire projectile at target
            FireForward();

			// Set cooldown for the skill
            currentProjectileCooldown = projectileCooldown;
        }*/

	}

	private void FireProjectile(Vector2 targetDir) {

		if(maximumTotalAllowedOnScreen > 0) {
			maximumTotalAllowedOnScreen--;
		} else {
			return;
		}

		// Create a projectile
		GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));

		// Calculate character's shooting direction
		Vector3 screenMousePos = Input.mousePosition;
		screenMousePos.z = Camera.main.transform.position.z;
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);
		Vector2 facingVector = (mouseWorldPos - firingPivot.position);
		facingVector = targetDir;

		// Setup projectile attribute (like damage, speed, etc)
		//Debug.Assert(newProjectile.GetComponent<ExampleLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<Boomerang>().SetupProjectile(projectileDamage * scorpionAttributes.DamageOutputFactorMultiplier, projectileSpeed, projectileLifeSpan, facingVector, gameObject, null);

		// Use this if using projectileBuff
		//newProjectile.GetComponent<ExampleLinearProjectile>().SetupProjectile(projectileDamage, projectileSpeed, projectileLifeSpan, facingVector, projectileBuff);
	}

	private void FireForward() {

		if(maximumTotalAllowedOnScreen > 0) {
			maximumTotalAllowedOnScreen--;
		} else {
			return;
		}

		// Create a projectile
        GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));

		// Calculate character's shooting direction
        Vector2 facingVector = firingIndicator.position - firingPivot.position;

		// Setup projectile attribute (like damage, speed, etc)
        //Debug.Assert(newProjectile.GetComponent<ExampleHomingProjectile>(), "Projectile does not contain the HomingProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<Boomerang>().SetupProjectile(projectileDamage, projectileSpeed, projectileLifeSpan, facingVector, gameObject, null);
	}

}
