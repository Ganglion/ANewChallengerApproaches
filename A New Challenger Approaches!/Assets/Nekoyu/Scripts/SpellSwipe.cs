using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SpellSwipe : MonoBehaviour {
	public int playerID;
	public Player nekoyuPlayer;
	[System.NonSerialized]
	private bool initialized;

	private Animator characterAnimator;

    // Prefabs
    [SerializeField]
    private GameObject projectile;

    // Objects
    [SerializeField]
    private Transform nearIndicator;
    [SerializeField]
    private Transform nearPivot;
	[SerializeField]
	private Transform mediumIndicator;
	[SerializeField]
	private Transform mediumPivot;
	[SerializeField]
	private Transform farIndicator;
	[SerializeField]
	private Transform farPivot;

    // Fields
    [SerializeField]
    private float projectileDamage;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileLifeSpan;
    [SerializeField]
    private float projectileCooldown;
	[SerializeField]
	private bool penetration;

    // Runtime variables
    private float nearProjectileCooldown = 0;
	private float mediumProjectileCooldown = 0;
	private float farProjectileCooldown = 0;
	private bool isBuffed = false;
	private UnitAttributes unitAttributes;
	private Vector3 currentNearIndicator;
	private Vector3 currentNearPivot;
	private Vector3 currentMediumIndicator;
	private Vector3 currentMediumPivot;
	private Vector3 currentFarIndicator;
	private Vector3 currentFarPivot;
	public static bool hitEnemy;

	private void Start () {
		unitAttributes = GetComponent<UnitAttributes>();
	}

    private void Update () {
		if(!ReInput.isReady) return;
		if(!initialized) nekoyuPlayer = ReInput.players.GetPlayer(playerID);

		if (BuffSkill.isBuffed == true && isBuffed == false) {
			isBuffed = true;
		}
		if (BuffSkill.isBuffed == false && isBuffed == true) {
			isBuffed = false;
		}

		if (isBuffed && hitEnemy) {
			if (unitAttributes.CurrentHealth < unitAttributes.BaseMaxHealth) {
				unitAttributes.Heal(projectileDamage * unitAttributes.DamageOutputFactorMultiplier * BuffSkill.lifestealAmount);
			}
			hitEnemy = false;
		}

		characterAnimator = GetComponent<Animator> ();
		if (nearProjectileCooldown > 0) {
            nearProjectileCooldown -= Time.deltaTime;
        }

		if (mediumProjectileCooldown > 0) {
			mediumProjectileCooldown -= Time.deltaTime;
		}

		if (farProjectileCooldown > 0) {
			farProjectileCooldown -= Time.deltaTime;
		}

		if (nekoyuPlayer.GetButton("Up Button") && nearProjectileCooldown <= 0 && NekoyuInput.attacking != true) {
			NekoyuInput.attacking = true;
			characterAnimator.Play ("Spellswipe", 0, 0f);
			nearProjectileCooldown = projectileCooldown;
			currentNearIndicator = nearIndicator.position;
			currentNearPivot = nearPivot.position;
			StartCoroutine ("NearProjectile");
		}

		if (nekoyuPlayer.GetButton("R Bumper") && mediumProjectileCooldown <= 0 && NekoyuInput.attacking != true) {
			NekoyuInput.attacking = true;
			characterAnimator.Play ("Spellswipe", 0, 0f);
			mediumProjectileCooldown = projectileCooldown;
			currentMediumIndicator = mediumIndicator.position;
			currentMediumPivot = mediumPivot.position;
			StartCoroutine ("MediumProjectile");
		}

		if (nekoyuPlayer.GetButton("RT Fire") && farProjectileCooldown <= 0 && NekoyuInput.attacking != true) {
			NekoyuInput.attacking = true;
			characterAnimator.Play ("Spellswipe", 0, 0f);
			farProjectileCooldown = projectileCooldown;
			currentFarIndicator = farIndicator.position;
			currentFarPivot = farPivot.position;
			StartCoroutine ("FarProjectile");
		}
	}

    IEnumerator NearProjectile() {
		yield return new WaitForSeconds (0.3f);
		NekoyuInput.attacking = false;
		yield return new WaitForSeconds (1.2f);
		GameObject newProjectile = (GameObject) Instantiate(projectile, currentNearIndicator, Quaternion.Euler(Vector3.zero));
        Vector2 facingVector = currentNearIndicator - currentNearPivot;
        Debug.Assert(newProjectile.GetComponent<NekoyuLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<NekoyuLinearProjectile>().SetupProjectile(penetration, "fish", projectileDamage * unitAttributes.DamageOutputFactorMultiplier, projectileSpeed, projectileLifeSpan, facingVector);
    }

	IEnumerator MediumProjectile() {
		yield return new WaitForSeconds (0.3f);
		NekoyuInput.attacking = false;
		yield return new WaitForSeconds (1.2f);
		GameObject newProjectile = (GameObject) Instantiate(projectile, currentMediumIndicator, Quaternion.Euler(Vector3.zero));
		Vector2 facingVector = currentMediumIndicator - currentMediumPivot;
		Debug.Assert(newProjectile.GetComponent<NekoyuLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<NekoyuLinearProjectile>().SetupProjectile(penetration, "fish", projectileDamage * unitAttributes.DamageOutputFactorMultiplier, projectileSpeed, projectileLifeSpan, facingVector);
	}

	IEnumerator FarProjectile() {
		yield return new WaitForSeconds (0.3f);
		NekoyuInput.attacking = false;
		yield return new WaitForSeconds (1.2f);
		GameObject newProjectile = (GameObject) Instantiate(projectile, currentFarIndicator, Quaternion.Euler(Vector3.zero));
		Vector2 facingVector = currentFarIndicator - currentFarPivot;
		Debug.Assert(newProjectile.GetComponent<NekoyuLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<NekoyuLinearProjectile>().SetupProjectile(penetration, "fish", projectileDamage * unitAttributes.DamageOutputFactorMultiplier, projectileSpeed, projectileLifeSpan, facingVector);
	}
}
