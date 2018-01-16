using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterMovement : UnitInput {

	public int playerID;
	public Player scorpionPlayer;
	[System.NonSerialized]
	private bool initialized;

	// Components
	protected Animator characterAnimator;
	protected SpriteRenderer spriteRenderer;
	protected UseProjectileSkill boomerangSkill;
	protected ScorpionAttribute scorpionAttribute;

	public int totalJumpsAllowed;
	public float glideRate;
	public float damageMultiplier;
	public ParticleSystem glideEffects;	

	// Jump variables
	private int totalJumps;
	private bool isGliding;
	private bool upButtonReleased;

	public bool isBlocking;
	public float blockingRecoveryTime;
	protected float blockingRecovery;

	//Jump = 0
	//Block = 1
	//Boomerang = 2
	public int lastUsedSkill;
	public int upgradedSkill;
	public float gravityMultiplierWithFlight;
	public float flightModeMultiplier;
	protected float normalGravityMultiplier = 1;
	public float totalFlightTime;
	protected float currentFlightTime = 0;
	protected bool isFlying = false;

	public GameObject flightUpgradeAnimation;
	public GameObject blockUpgradeAnimation;
	public GameObject boomerangUpgradeAnimation;

	public float upgradeBlockingMovementMultiplier = 0.1f;

	// Used for animations
	protected bool isFacingRight = true;
	public bool IsFacingRight { get { return isFacingRight; } }

	protected override void Awake() {
		base.Awake ();
		characterAnimator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		boomerangSkill = GetComponent<UseProjectileSkill>();
		scorpionAttribute = GetComponent<ScorpionAttribute>();
		totalJumps = totalJumpsAllowed;
		isGliding = false;
		upButtonReleased = true;
		isBlocking = false;
		blockingRecovery = blockingRecoveryTime;
	}

	protected void Update() {
		if(!ReInput.isReady) return;
		if(!initialized) scorpionPlayer = ReInput.players.GetPlayer(playerID);
		if(Input.GetKeyUp(KeyCode.Space) || scorpionPlayer.GetButtonUp ("LT") || (scorpionPlayer.GetAxis("Move Vertical") < .5f && !scorpionPlayer.GetButton ("LT"))) {
			upButtonReleased = true;
		}
	}
	
	protected void FixedUpdate() {
		if(!ReInput.isReady) return;
		// Execute this every frame
		DoPlayerInput();
	}

	protected bool jumpKeyPressed() {
		return Input.GetKey (KeyCode.Space) || scorpionPlayer.GetButton ("LT") || scorpionPlayer.GetAxis("Move Vertical") >= .5f;
	}

	protected void DoPlayerInput() {
		
		// Retrieves values from attributes every frame (as buffs/debuffs may change them)
		float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
		float jumpHeight = characterAttributes.CurrentJumpHeight;

		bool hasMovedHorizontally = false;

		// Character not on ground?
		if (!characterMovement.collisions.below) {
			if(upgradedSkill == 0 && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && currentFlightTime <= totalFlightTime) {
				currentFlightTime += Time.deltaTime;
				currentVelocity.y = flightModeMultiplier * 5;
				if(!isFlying) {
					isFlying = true;
					glideEffects.Play();
				}
			} else {

				isFlying = false;

				// fall
				// Gravity is multiplied by 5 to make the jump less floaty
				if(currentVelocity.y <= 0 && jumpKeyPressed() && (!upButtonReleased || upgradedSkill == 0)) {
					if(!isGliding) {
						isGliding = true;
						glideEffects.Play();
					}
					currentVelocity.y = glideRate * -1;
				} else {
					currentVelocity.y += Time.deltaTime * -9.81f * 5 * normalGravityMultiplier;
					isGliding = false;
					glideEffects.Stop();
				}
			}
		
		// otherwise,
		} else {

			// don't fall
			currentVelocity.y = 0f;
			totalJumps = totalJumpsAllowed;
			isGliding = false;
			currentFlightTime = 0;
		}

		if  (Input.GetKey(KeyCode.X) && currentVelocity.y == 0 || scorpionPlayer.GetButtonDown("R Bumper")) {
			//crouch, reduce damage
			isBlocking = true;
			blockingRecovery = blockingRecoveryTime;
			gameObject.GetComponent<ScorpionAttribute>().damageMultiplier = this.damageMultiplier;
			lastUsedSkill = 1;
		} else {
			blockingRecovery -= Time.deltaTime;
			if(blockingRecovery <= 0) {
				isBlocking = false;
				gameObject.GetComponent<ScorpionAttribute>().damageMultiplier = 1;
			}
		}

		// Pressed left arrow?
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || scorpionPlayer.GetAxis("Move Horizontal") < 0) {

			if(!isBlocking) {
				// Accelerate leftwards towards max speed
				currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
				hasMovedHorizontally = true;
				isFacingRight = false;
				characterAnimator.SetBool("isMoving", true);
			} else if(upgradedSkill == 1) {
				currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed * upgradeBlockingMovementMultiplier, currentAcceleration * Time.deltaTime);
				hasMovedHorizontally = true;
				isFacingRight = false;
				characterAnimator.SetBool("isMoving", true);
			}
		}

		// Pressed right arrow?
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || scorpionPlayer.GetAxis("Move Horizontal") > 0) {

			if(!isBlocking) {
				// Accelerate rightwards towards max speed
				currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
				hasMovedHorizontally = true;
				isFacingRight = true;
				characterAnimator.SetBool("isMoving", true);
			} else if(upgradedSkill == 1) {
				currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed * upgradeBlockingMovementMultiplier, currentAcceleration * Time.deltaTime);
				hasMovedHorizontally = true;
				isFacingRight = true;
				characterAnimator.SetBool("isMoving", true);
			}
		}

		// Pressed up arrow?
		if (jumpKeyPressed() && upButtonReleased) {
			isBlocking = false;
			if(!isGliding) {
				upButtonReleased = false;
			}

			// If character is on ground,
			if (characterMovement.collisions.below) {

				// jump!
				// The equation below calculates the velocity required to reach target jump height.
				// Multiplied by 5 to account for the 5x gravity.
				currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5);
			}
			else if(totalJumps > 0) {
				currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5);
			}
			totalJumps--;
			lastUsedSkill = 0;
		}

		// Didn't move left or right?
		if (!hasMovedHorizontally) {

			// Decelerate towards 0 speed
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
			if(currentVelocity.x < 0.0001) {
				characterAnimator.SetBool("isMoving", false);
			}
		}

		if(!isBlocking) {

			// Click to fire projectile at mouse
			if (Input.GetMouseButtonDown(0) || scorpionPlayer.GetButtonDown("RT Fire")) {

				Vector2 rsDirection = new Vector2 (scorpionPlayer.GetAxis ("RS Horizontal"), scorpionPlayer.GetAxis ("RS Vertical"));
				//Debug.Log (rsDirection);
				boomerangSkill.FireProjectile(rsDirection, upgradedSkill == 2);
				lastUsedSkill = 2;
			}

			// Pressed C button to fire projectile forward.
			else if (Input.GetKey(KeyCode.C)) {

				// Fire projectile at target
				boomerangSkill.FireProjectile(Vector2.zero, upgradedSkill == 2);
				lastUsedSkill = 2;
			}
		}

		spriteRenderer.flipX = !isFacingRight;

			
		// All velocity calculated, time to move the player
		// Pressed down button?
		if (Input.GetKey(KeyCode.DownArrow) || scorpionPlayer.GetAxis("Move Vertical") <= 0.5f) {

			// move player while passing through platforms
			// Use Vector2.down as the second parameter if you want to pass through platforms
			characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.down);

		// otherwise
		} else {
			
			// move player normally
			characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
		}
	}

	public void upgradeSkill() {
		upgradedSkill = lastUsedSkill;
		if(upgradedSkill == 0) {
			normalGravityMultiplier = gravityMultiplierWithFlight;
			totalJumpsAllowed = 1;
			Instantiate(flightUpgradeAnimation, gameObject.transform.position, Quaternion.identity);
		} else if(upgradedSkill == 1) {
			scorpionAttribute.gainDefences();
			Instantiate(blockUpgradeAnimation, gameObject.transform.position, Quaternion.identity);
		} else if(upgradedSkill == 2) {
			boomerangSkill.projectileCooldown = 2;
			Instantiate(boomerangUpgradeAnimation, gameObject.transform.position, Quaternion.identity);
		}
	}

}