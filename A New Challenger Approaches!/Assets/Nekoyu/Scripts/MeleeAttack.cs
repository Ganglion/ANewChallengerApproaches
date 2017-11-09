using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MeleeAttack : MonoBehaviour {

	public int playerID;
	public Player nekoyuPlayer;
	[System.NonSerialized]
	private bool initialized;

	protected Animator characterAnimator;
	private int alternateAnimation = 1;
	public UnitAttributes unitAttributes;

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
	[SerializeField]
	private bool penetration;

	// Runtime variables
	private float currentProjectileCooldown = 0;
	private bool isBuffed = false;
	public static bool hitEnemy;

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

		characterAnimator = GetComponent<Animator>();
		if (currentProjectileCooldown > 0) {
			currentProjectileCooldown -= Time.deltaTime;
		}

		//if (Input.GetKeyDown(KeyCode.C) && currentProjectileCooldown <= 0) {
		if (nekoyuPlayer.GetButton("Fire") && currentProjectileCooldown <= 0) {
			NekoyuInput.attacking = true;
			FireProjectile ();
			StartCoroutine ("postAttackDelay");
			currentProjectileCooldown = projectileCooldown;
		}
	}

	IEnumerator postAttackDelay(){
		yield return new WaitForSeconds (0.2f);
		NekoyuInput.attacking = false;
	}

	private void FireProjectile() {
		if (characterAnimator.GetInteger ("Movement") == 2) {
			characterAnimator.Play ("Jumpslash", 0, 0f);
		} else {
			if (alternateAnimation == 1) {
				characterAnimator.Play ("Slash", 0, 0f);
				alternateAnimation = 0;
			} else {
				characterAnimator.Play ("Jab", 0, 0f);
				alternateAnimation = 1;
			}

		}
		StartCoroutine ("preAttackDelay");
	}
	IEnumerator preAttackDelay(){
		yield return new WaitForSeconds (0.1f);
		GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));
		Vector2 facingVector = firingIndicator.position - firingPivot.position;
		Debug.Assert(newProjectile.GetComponent<NekoyuLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<NekoyuLinearProjectile>().SetupProjectile(penetration, "melee", projectileDamage * unitAttributes.DamageOutputFactorMultiplier, projectileSpeed, projectileLifeSpan, facingVector);
	}
}