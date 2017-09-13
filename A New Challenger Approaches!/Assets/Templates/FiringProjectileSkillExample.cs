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

    // Runtime variables
    private float currentProjectileCooldown = 0;

    private void Update () {
        if (currentProjectileCooldown > 0) {
            currentProjectileCooldown -= Time.deltaTime;
        }

		if (Input.GetKey(KeyCode.C) && currentProjectileCooldown <= 0) {
            FireProjectile();
            currentProjectileCooldown = projectileCooldown;
        }
	}

    private void FireProjectile() {
        GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));
        Vector2 facingVector = firingIndicator.position - firingPivot.position;
        Debug.Assert(newProjectile.GetComponent<ExampleLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
        newProjectile.GetComponent<ExampleLinearProjectile>().SetupProjectile(projectileDamage, projectileSpeed, projectileLifeSpan, facingVector);
    }
}
