using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Lifesteal : MonoBehaviour {

	public int playerID;
	public Player nekoyuPlayer;
	[System.NonSerialized]
	private bool initialized;

	private Animator characterAnimator;
	protected UnitAttributes unitAttributes;

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
	private bool penetration;
    [SerializeField]
    private float skillCooldown;
	[SerializeField]
	private float maxSkillDuration;
	[SerializeField]
	private float healingAmount;

    // Runtime variables
    private float currentSkillCooldown = 0;
	private float currentSkillDuration = 0;
	private bool addCooldown = false;
	public static bool hitEnemy;
	private bool isBuffed = false;

	private void Start () {
		unitAttributes = GetComponent<UnitAttributes> ();
	}

    private void FixedUpdate () {
		if(!ReInput.isReady) return;
		if(!initialized) nekoyuPlayer = ReInput.players.GetPlayer(playerID);

		if (BuffSkill.isBuffed == true && isBuffed == false) {
			isBuffed = true;
		}
		if (BuffSkill.isBuffed == false && isBuffed == true) {
			isBuffed = false;
		}

		characterAnimator = GetComponent<Animator> ();
		if (currentSkillCooldown > 0) {
            currentSkillCooldown -= Time.deltaTime;
        }

		//if (Input.GetKey (KeyCode.Z) && currentSkillCooldown <= 0 && !NekoyuInput.inAir) {
		if (nekoyuPlayer.GetButton("Left Button") && currentSkillCooldown <= 0 && !NekoyuInput.inAir) {
			if (currentSkillDuration < maxSkillDuration) {
				if (characterAnimator.GetInteger ("Attack") != 3) {
					characterAnimator.Play ("LifestealStart", 0, 0f);
				}
				characterAnimator.SetInteger ("Attack", 3);
				NekoyuInput.attacking = true;
				FireProjectile (); 
				addCooldown = true;
				currentSkillDuration += Time.deltaTime;
			}
		}
		//if (Input.GetKeyUp (KeyCode.Z) || currentSkillDuration >= maxSkillDuration) {
		if (nekoyuPlayer.GetButton("Left Button") || currentSkillDuration >= maxSkillDuration) {
			characterAnimator.SetInteger ("Attack", 0);
			StartCoroutine ("waitForSeconds");
			if (addCooldown == true){
				currentSkillCooldown = skillCooldown;
				addCooldown = false;
				currentSkillDuration = 0;
			}
		}
		if (hitEnemy == true) {
			unitAttributes.Heal(healingAmount);
		}
		hitEnemy = false;	
	}


	IEnumerator waitForSeconds(){
		yield return new WaitForSeconds (0.4f);
		NekoyuInput.attacking = false;
	}



    private void FireProjectile() {
		StartCoroutine ("preAttackDelay");
        
    }

	IEnumerator preAttackDelay(){
		yield return new WaitForSeconds (0.4f);
		GameObject newProjectile = (GameObject) Instantiate(projectile, firingIndicator.position, Quaternion.Euler(Vector3.zero));
		Vector2 facingVector = firingIndicator.position - firingPivot.position;
		Debug.Assert(newProjectile.GetComponent<NekoyuLinearProjectile>(), "Projectile does not contain the LinearProjectile component. Check if you getting the correct component.");
		newProjectile.GetComponent<NekoyuLinearProjectile>().SetupProjectile(penetration, "lifesteal", projectileDamage * unitAttributes.DamageOutputFactorMultiplier, projectileSpeed, projectileLifeSpan, facingVector);
	}

}
