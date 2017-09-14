using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeguardController : UnitInput {

    // Constants
    protected const string READY_TRIGGER = "BridgeguardReady";
    protected const string WALK_TRIGGER = "BridgeguardWalk";
    protected const string STAB_TRIGGER = "BridgeguardStab";
    protected const string SPITFIRE_TRIGGER = "BridgeguardSpitfire";
    protected const string PLAYER_LAYER = "Player";

    // References
    [SerializeField]
    protected Transform targetCharacter;
    [SerializeField]
    protected GameObject spitfireProjectile;
    [SerializeField]
    protected Transform spitfireTransform;
	[SerializeField]
	protected GameObject shockwaveProjectile;
	[SerializeField]
	protected Transform shockwaveTransform;

	[Header("AI")]
    // Fields
    [SerializeField]
    protected int moveWeight;
    [SerializeField]
    protected int stabWeight;
    [SerializeField]
    protected int spitfireWeight;

	[Header("Spitfire")]
    // Spitfire Fields
    [SerializeField]
    protected float spitfireDamage;
    [SerializeField]
    protected float spitfireHeight;
    [SerializeField]
    protected float spitfireGravity;
    [SerializeField]
    protected float spitfireLifespan;
    [SerializeField]
    protected float spitfireDamageOverTime;
    [SerializeField]
    protected float spitfireDamageOverTimeDuration;
	[SerializeField]
	protected GameObject burningEffect;

	[Header("Shockwave")]
	// Shockwave fields
	[SerializeField]
	protected float shockwaveDamage;
	[SerializeField]
	protected float shockwaveSpeed;
	[SerializeField]
	protected float shockwaveLifespan;

	[Header("Delay")]
    // Delay
    [SerializeField]
    protected float minCoolown;
    [SerializeField]
    protected float maxCooldown;

	[Header("Pre-Combat")]
    // Precombat
    [SerializeField]
    protected float detectionRadius;

    // Runtime variables
    protected bool hasEnteredCombat = false;
    protected bool isDoingAction = false;
    protected float cooldownToNextAction = 0;
    protected int totalWeights;
    protected float initialScale;
	protected bool isDead = false;

    // Attack buffs
    protected const string spitfireDamageOverTimeBuffName = "Spitfire Damage Over Time";
    protected DamageOverTimeBuff spitfireDamageOverTimeBuff;

    // Components
    protected Animator characterAnimator;

    protected override void Awake() {
        base.Awake();
        characterAnimator = GetComponent<Animator>();
        totalWeights = moveWeight + stabWeight + spitfireWeight;
        initialScale = transform.localScale.x;
		spitfireDamageOverTimeBuff = new DamageOverTimeBuff(spitfireDamageOverTime, spitfireDamageOverTimeBuffName, spitfireDamageOverTimeDuration, burningEffect, false);
    }

    protected void FixedUpdate() {
        if (!hasEnteredCombat) {
            RaycastHit2D detectedPlayer = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.zero, 0, LayerMask.GetMask(PLAYER_LAYER));
            if (detectedPlayer.collider != null) {
                characterAnimator.SetTrigger(READY_TRIGGER);
                CameraController.Instance.AddToCameraTracker(transform);
                StartCoroutine(ActivateReadyState());
            }
		} else if (!isDead) {
            cooldownToNextAction -= Time.deltaTime;

            float movementSpeed = characterAttributes.CurrentMovementSpeed;
            float currentAcceleration = characterAttributes.CurrentGroundAcceleration;

			characterAnimator.speed = characterAttributes.MovementSpeedMultiplier;

            if (!characterMovement.collisions.below) { // Is not on ground?
                currentVelocity.y += Time.deltaTime * -9.81f * 5; // Fall
            } else {
                currentVelocity.y = 0f;
            }

            if (!isDoingAction && cooldownToNextAction <= 0 && characterAttributes.CanExecuteActions && hasEnteredCombat) {
                int chosenAction = Random.Range(0, totalWeights);
                if (chosenAction < moveWeight) {
                    characterAnimator.SetTrigger(WALK_TRIGGER);
					currentVelocity.x = movementSpeed * FaceDirectionToTarget() * characterAttributes.MovementSpeedMultiplier;

                } else if ((chosenAction - moveWeight) < stabWeight) {
                    FaceDirectionToTarget();
                    characterAnimator.SetTrigger(STAB_TRIGGER);
                } else if ((chosenAction - moveWeight - stabWeight) < spitfireWeight) {
                    FaceDirectionToTarget();
                    characterAnimator.SetTrigger(SPITFIRE_TRIGGER);
                }

                isDoingAction = true;
                StartCoroutine(ApplyCooldownDelay());
            }
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime);
            characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
        }
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

    protected IEnumerator ActivateReadyState() {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
        hasEnteredCombat = true;
        cooldownToNextAction = Random.Range(minCoolown, maxCooldown);
    }

    protected IEnumerator ApplyCooldownDelay() {
        isDoingAction = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
		isDoingAction = false;
		cooldownToNextAction = Random.Range (minCoolown, maxCooldown);
	}

    protected void ReadyLanding() {
        CameraController.Instance.ShakeCamera(.25f, 2f);
    }

	protected void LaunchSpitfire() {
		CameraController.Instance.ShakeCamera (.125f, .75f);
		Vector2 targetPosition = targetCharacter.position;
        float verticalSpeed = Mathf.Sqrt(2 * Mathf.Abs(spitfireGravity) * spitfireHeight);
		float totalTimeTaken = verticalSpeed / Mathf.Abs (spitfireGravity) + Mathf.Sqrt(2 * (spitfireHeight + spitfireTransform.position.y - targetPosition.y) / Mathf.Abs (spitfireGravity)); 
		float horizontalSpeed = (targetPosition.x - spitfireTransform.position.x) / totalTimeTaken;
        GameObject newSpitfire = (GameObject)Instantiate(spitfireProjectile, spitfireTransform.position, Quaternion.Euler(Vector3.zero));
		newSpitfire.GetComponent<SpitfireProjectile>().SetupProjectile(spitfireDamage, new Vector2(horizontalSpeed, verticalSpeed), spitfireLifespan, spitfireGravity, spitfireDamageOverTimeBuff);
    }

	protected void LaunchShockwave() {
		CameraController.Instance.ShakeCamera (.125f, .75f);
		Vector2 facingVector = new Vector2 (Mathf.Sign (shockwaveTransform.position.x - transform.position.x), 0);
		GameObject newShockwave = (GameObject)Instantiate (shockwaveProjectile, shockwaveTransform.position, Quaternion.Euler (Vector3.zero));
		newShockwave.GetComponent<ShockwaveProjectile> ().SetupProjectile (shockwaveDamage, shockwaveSpeed, shockwaveLifespan, facingVector, null);
	}

	public void SetDead() {
		isDead = true;
	}

	protected void Death() {
		this.gameObject.SetActive (false);
	}

}
