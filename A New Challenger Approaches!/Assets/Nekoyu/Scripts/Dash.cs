using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Dash : MonoBehaviour {

	public int playerID;
	public Player nekoyuPlayer;
	[System.NonSerialized]
	private bool initialized;

	protected const string ENEMY_LAYER = "Enemy";
	protected const string DESTRUCTIBLE_LAYER = "Destructible";
	protected const string STRUCTURE_LAYER = "Structure";
	protected const string PLAYER_LAYER = "Player";

	protected Animator characterAnimator;
	protected BoxCollider2D boxcollider;
	protected UnitAttributes unitAttributes;
	private float currentDashCooldown = 0f; //I removed dashTimer and canDash, and replaced them with this.
	[SerializeField]
	private float dashSpeed; //dashDuration replaced with this.
	[SerializeField]
	private float dashDuration;
	[SerializeField]
	private float dashDamage;
	private float currentDashDuration;
	public float dashCooldown;
	private bool facingRight;
	private bool isDashing;
	private bool isBuffed = false;
	private Buff dashBuff;

	// Use this for initialization

	void Start () {
		characterAnimator = GetComponent<Animator> ();
		boxcollider = GetComponent<BoxCollider2D>();
		currentDashDuration = dashDuration;
		unitAttributes = GetComponent<UnitAttributes> ();
		dashBuff = new DashBuff ("Dash Invulnerability", dashDuration, null, false);
	}

	private void startDash () {//Sounds like a song...
		currentDashDuration -= Time.deltaTime;
		NekoyuInput.attacking = true; //Meant to disable input, but you can still change direction mid-dash. Might fix.
		if (currentDashDuration > 0f && facingRight) {
			GetComponent<ObjectMovement> ().Move (new Vector2 (dashSpeed * Time.deltaTime, 0), false);
			//transform.position += new Vector3 (dashSpeed * Time.deltaTime, 0, 0);
			boxcollider.isTrigger = true; //Used to set the collision level of the player
			//If you wish, you can get the rigidbody2D component and set gravity to 0 during the duration of the dash
			//Because right now, dash passes through walls, but also through floors.
		} else if (currentDashDuration > 0f && !facingRight) {
			GetComponent<ObjectMovement> ().Move (new Vector2 (-dashSpeed * Time.deltaTime, 0), false);
			//transform.position += new Vector3 (-dashSpeed * Time.deltaTime, 0, 0);
			boxcollider.isTrigger = true; 
		}
	}

	//Stole this off Luke's code

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		GameObject hitObject = other.gameObject;
		if (hitObject.layer == LayerMask.NameToLayer (ENEMY_LAYER)) {
			OnHitEnemy (hitObject);
		} else if (hitObject.layer == LayerMask.NameToLayer (DESTRUCTIBLE_LAYER)) {
			OnHitDestructible (hitObject);
		}
	}

	protected virtual void OnHitEnemy(GameObject hitObject) {
		if (isDashing) {
			hitObject.GetComponent<UnitAttributes> ().ApplyAttack (dashDamage * unitAttributes.DamageOutputFactorMultiplier, transform.position);
			if (isBuffed) {
				unitAttributes.Heal (dashDamage * unitAttributes.DamageOutputFactorMultiplier * BuffSkill.lifestealAmount);
			}
		}
	}


	protected virtual void OnHitDestructible(GameObject hitObject) {
		OnHitEnemy(hitObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(!ReInput.isReady) return;
		if(!initialized) nekoyuPlayer = ReInput.players.GetPlayer(playerID);

		if (BuffSkill.isBuffed == true && isBuffed == false) {
			isBuffed = true;
		}
		if (BuffSkill.isBuffed == false && isBuffed == true) {
			isBuffed = false;
		}

		if (transform.localScale.x > 0) {
			facingRight = true;
		} else {
			facingRight = false;
		}
			
		if (nekoyuPlayer.GetButton("Right Button") && currentDashCooldown <= 0f) {
			characterAnimator.Play ("Dash", 0, 0f);
			isDashing = true;
			currentDashCooldown = dashCooldown;
			unitAttributes.ApplyAttack (0, transform.position, dashBuff);
		}

		if (isDashing && currentDashDuration > 0f) {
			startDash ();
		}

		if (currentDashDuration <= 0f) {
			boxcollider.isTrigger = false;
			currentDashDuration = dashDuration;
			isDashing = false;
			NekoyuInput.attacking = false; 
		}

		if (currentDashCooldown > 0) {
			currentDashCooldown -= Time.deltaTime;
		}
	} 
}
	