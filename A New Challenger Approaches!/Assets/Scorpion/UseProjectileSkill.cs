using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseProjectileSkill : MonoBehaviour {
	// Prefabs
	[SerializeField]
	private GameObject projectile;

	// Objects
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
	public float projectileCooldown;
	public float timeBeforeBoomerangReturn;

	protected SpriteRenderer shooterSprite;
	protected CharacterMovement characterInformation;

	private MovementSpeedBuff projectileBuff;
	public int maximumTotalAllowedOnScreen;

	public float upgradeSecondProjectileSpeed;
	public float upgradeSecondProjectileDelay;
	public float upgradeSecondProjetileReturnTime;

	public float upgradeThirdProjectileSpeed;
	public float upgradeThirdProjectileDelay;
	public float upgradeThirdProjetileReturnTime;


	private void Awake() {

		// Uncomment this (and put projectileBuff as an argument in SetupProjectile()) to apply a slow buff to the example character's projectiles
		//projectileBuff = new MovementSpeedBuff (.75f, "EXAMPLE_SLOW", 3, null, false);
		shooterSprite = gameObject.GetComponent<SpriteRenderer>();
		characterInformation = gameObject.GetComponent<CharacterMovement>();
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
	}

	private bool fireCheck() {
		if(maximumTotalAllowedOnScreen > 0 && currentProjectileCooldown <= 0) {
			maximumTotalAllowedOnScreen--;
			currentProjectileCooldown = projectileCooldown;
			return true;
		} else {
			return false;
		}
	}

	public void FireProjectile(Vector2 dir, bool isUpgraded) {
		if(!fireCheck()) {
			return;
		}

		Vector2 facingVector = new Vector2(0, 0);

		if (dir != Vector2.zero) {
			// Calculate character's shooting direction
			/*
			Vector3 screenMousePos = Input.mousePosition;
			screenMousePos.z = Camera.main.transform.position.z;
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);
			facingVector = (mouseWorldPos - firingPivot.position);*/
			facingVector = dir;
		} else {
			facingVector = new Vector2(1, 0);
			if(shooterSprite.flipX) {
				facingVector = new Vector2(-1, 0);
			}
		}

		StartCoroutine (FireBoomerang(facingVector, projectileSpeed, 0, timeBeforeBoomerangReturn));

		if(isUpgraded) {
			StartCoroutine (FireBoomerang(facingVector, upgradeSecondProjectileSpeed, upgradeSecondProjectileDelay, upgradeSecondProjetileReturnTime));
			StartCoroutine (FireBoomerang(facingVector, upgradeThirdProjectileSpeed, upgradeThirdProjectileDelay, upgradeThirdProjetileReturnTime));
		}
	}

	private IEnumerator FireBoomerang(Vector2 direction, float speed, float waitTime, float returnTime) {

		yield return new WaitForSeconds(waitTime);
		// Create a projectile
		GameObject newProjectile = (GameObject) Instantiate(projectile, gameObject.transform.position, Quaternion.Euler(Vector3.zero));

		// Setup projectile attribute (like damage, speed, etc)
		//Debug.Assert(newProjectile.GetComponent<ExampleLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<Boomerang>().SetupProjectile(projectileDamage, speed, projectileLifeSpan, direction, gameObject, returnTime, null);

		yield return null;
	}
}
