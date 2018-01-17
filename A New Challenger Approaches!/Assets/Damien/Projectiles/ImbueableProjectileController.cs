using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImbueableProjectileController : MonoBehaviour {

    [SerializeField]
    GameObject imbuedProjectilePrefab;

    ExampleLinearProjectile original;

    // Upgrade Values
    [Header("Imbue Values")]
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speedChange;
    [SerializeField]
    private float lifeSpan; // original PLUS lifeSpan

    private void Awake()
    {
        original = GetComponent<ExampleLinearProjectile>();
    }

    public void Imbue(){
        // Create a projectile
        GameObject newProjectile = (GameObject)Instantiate(imbuedProjectilePrefab, transform.position, Quaternion.Euler(Vector3.zero));

        // Setup projectile attribute (like damage, speed, etc)
        Debug.Assert(newProjectile.GetComponent<ExampleLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
        newProjectile.GetComponent<ExampleLinearProjectile>().SetupProjectile(original.projectileDamage + damage, original.projectileSpeed + speedChange, original.projectileLifespan + lifeSpan, original.unitProjectileDirection, null);
        Destroy(gameObject);
    }
}
