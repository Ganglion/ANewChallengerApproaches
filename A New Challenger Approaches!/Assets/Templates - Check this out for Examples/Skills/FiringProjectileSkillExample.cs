using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringProjectileSkillExample : MonoBehaviour {

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

	private MovementSpeedBuff projectileBuff;

	private void Awake() {

		// Uncomment this (and put projectileBuff as an argument in SetupProjectile()) to apply a slow buff to the example character's projectiles
		//projectileBuff = new MovementSpeedBuff (.75f, "EXAMPLE_SLOW", 3, null, false);
	}

    // Runtime variables
    private float currentProjectileCooldown = 0;

	// Executes every frame
    private void Update () {

		// Skill is on cooldown?
        if (currentProjectileCooldown > 0) {

			// Decrease cooldown time according to frame time
            currentProjectileCooldown -= Time.deltaTime;
        }

		// Pressed C button and skill is off cooldown?
		if (Input.GetKey(KeyCode.C) && currentProjectileCooldown <= 0) {
			
            FireProjectile();

			// Set cooldown for the skill
            currentProjectileCooldown = projectileCooldown;
        }
	}

    private void FireProjectile() {

		// Create a projectile
        GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));

		// Calculate character's shooting direction
        Vector2 facingVector = firingIndicator.position - firingPivot.position;

		// Setup projectile attribute (like damage, speed, etc)
        Debug.Assert(newProjectile.GetComponent<ExampleLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<ExampleLinearProjectile>().SetupProjectile(projectileDamage, projectileSpeed, projectileLifeSpan, facingVector, null);

		// Use this if using projectileBuff
		//newProjectile.GetComponent<ExampleLinearProjectile>().SetupProjectile(projectileDamage, projectileSpeed, projectileLifeSpan, facingVector, projectileBuff);
    }

}
