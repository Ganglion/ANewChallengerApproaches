using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeguardController : UnitInput {

	// Constants
	protected const string WALK_TRIGGER = "BridgeguardWalk";
	protected const string STAB_TRIGGER = "BridgeguardStab";
	protected const string SPITFIRE_TRIGGER = "BridgeguardSpitfire";

	// References
	[SerializeField]
	protected Transform targetCharacter;

	// Fields
	[SerializeField]
	protected int moveWeight;
	[SerializeField]
	protected int stabWeight;
	[SerializeField]
	protected int spitfireWeight;
	[SerializeField]
	protected float minCoolown;
	[SerializeField]
	protected float maxCooldown;

	// Runtime variables
	protected bool isDoingAction = false;
	protected float cooldownToNextAction = 0;
	protected int totalWeights;
	protected float initialScale;

	// Components
	protected Animator characterAnimator;

	protected override void Awake() {
		base.Awake ();
		characterAnimator = GetComponent<Animator>();
		totalWeights = moveWeight + stabWeight + spitfireWeight;
		initialScale = transform.localScale.x;
	}

	protected void Update() {
		cooldownToNextAction -= Time.deltaTime;

		float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterAttributes.CurrentGroundAcceleration;

		if (!characterMovement.collisions.below) { // Is not on ground?
			currentVelocity.y += Time.deltaTime * -9.81f * 5; // Fall
		} else {
			currentVelocity.y = -0.1f;
		}

		//Debug.Log (isDoingAction + " " + cooldownToNextAction + " ");
		if (!isDoingAction && cooldownToNextAction <= 0 && characterAttributes.CanExecuteActions) {
			//ebug.Log ("ACTION");
			int chosenAction = Random.Range (0, totalWeights);
			if (chosenAction < moveWeight) {
				//Debug.Log ("WALK");
				characterAnimator.SetTrigger (WALK_TRIGGER);
				currentVelocity.x = movementSpeed * FaceDirectionToTarget();
			} else if ((chosenAction - moveWeight) < stabWeight) {
				FaceDirectionToTarget ();
				characterAnimator.SetTrigger (STAB_TRIGGER);
			} else if ((chosenAction - moveWeight - stabWeight) < spitfireWeight) {
				FaceDirectionToTarget ();
				characterAnimator.SetTrigger (SPITFIRE_TRIGGER);
			}

			isDoingAction = true;
			ApplyCooldown ();
		}
		currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime);
		characterMovement.Move (currentVelocity * Time.deltaTime, Vector2.zero);
	}

	protected int FaceDirectionToTarget() {
		//Debug.Log (targetCharacter.position.x - transform.position.x);
		bool targetIsToTheRight = ((targetCharacter.position.x - transform.position.x) > 0) ? true : false;
		if (targetIsToTheRight) {
			Vector3 transformScale = transform.localScale;
			transformScale.x = -initialScale;
			transform.localScale = transformScale;
			return 1;
		} else {
			Vector3 transformScale = transform.localScale;
			transformScale.x = initialScale;
			transform.localScale = transformScale;
			return -1;
		}
	}

	protected void ApplyCooldown() {
		isDoingAction = false;
		cooldownToNextAction = Random.Range (minCoolown, maxCooldown);
	}

	protected void DoPlayerInput() {
		// Retrieves values from attributes every frame (as buffs/debuffs may change them)
		float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
		float jumpHeight = characterAttributes.CurrentJumpHeight;
		bool hasMovedHorizontally = false;



		if (Input.GetKey(KeyCode.LeftArrow)) { // Move left
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
			hasMovedHorizontally = true;
			isFacingRight = false;
		}
		if (Input.GetKey(KeyCode.RightArrow)) { // Move right
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
			hasMovedHorizontally = true;
			isFacingRight = true;
		}
		if (Input.GetKey(KeyCode.UpArrow)) { // Jump
			if (characterMovement.collisions.below) {
				currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5); // Velocity to achieve ideal height
			}
		}

		if (!hasMovedHorizontally) {
			currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
		}

		// All velocity calculated, move the player according to velocity
		if (Input.GetKey(KeyCode.DownArrow)) {
			// Use Vector2.down as the second parameter if you want to pass through platforms
			characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.down);
		} else {
			characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
		}
	}

}
