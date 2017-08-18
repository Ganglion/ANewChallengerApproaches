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
    [SerializeField]
    protected GameObject spitfireProjectile;
    [SerializeField]
    protected Transform spitfireTransform;
	[SerializeField]
	protected GameObject shockwaveProjectile;
	[SerializeField]
	protected Transform shockwaveTransform;

    // Fields
    [SerializeField]
    protected int moveWeight;
    [SerializeField]
    protected int stabWeight;
    [SerializeField]
    protected int spitfireWeight;

    // Spitfire Fields
    [SerializeField]
    protected float spitfireDamage;
    [SerializeField]
    protected float spitfireHeight;
    [SerializeField]
    protected float spitfireGravity;
    [SerializeField]
    protected float spitfireLifespan;

	// Shockwave fields
	[SerializeField]
	protected float shockwaveDamage;
	[SerializeField]
	protected float shockwaveSpeed;
	[SerializeField]
	protected float shockwaveLifespan;

    // Delay
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
        base.Awake();
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
            int chosenAction = Random.Range(0, totalWeights);
            if (chosenAction < moveWeight) {
                characterAnimator.SetTrigger(WALK_TRIGGER);
                currentVelocity.x = movementSpeed * FaceDirectionToTarget();

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

	protected IEnumerator ApplyCooldownDelay() {
        isDoingAction = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
		isDoingAction = false;
		cooldownToNextAction = Random.Range (minCoolown, maxCooldown);
	}

	protected void LaunchSpitfire() {
		CameraController.Instance.ShakeCamera (.125f, .75f);
		Vector2 targetPosition = targetCharacter.position;
        float verticalSpeed = Mathf.Sqrt(2 * Mathf.Abs(spitfireGravity) * spitfireHeight);
		float totalTimeTaken = verticalSpeed / Mathf.Abs (spitfireGravity) + Mathf.Sqrt(2 * (spitfireHeight + spitfireTransform.position.y - targetPosition.y) / Mathf.Abs (spitfireGravity)); 
		float horizontalSpeed = (targetPosition.x - spitfireTransform.position.x) / totalTimeTaken;
        Debug.Log(horizontalSpeed + " " + verticalSpeed);
        GameObject newSpitfire = (GameObject)Instantiate(spitfireProjectile, spitfireTransform.position, Quaternion.Euler(Vector3.zero));
        newSpitfire.GetComponent<SpitfireProjectile>().SetupProjectile(spitfireDamage, new Vector2(horizontalSpeed, verticalSpeed), spitfireLifespan, spitfireGravity, null);

    }

	protected void LaunchShockwave() {
		CameraController.Instance.ShakeCamera (.125f, .75f);
		Vector2 facingVector = new Vector2 (Mathf.Sign (shockwaveTransform.position.x - transform.position.x), 0);
		GameObject newShockwave = (GameObject)Instantiate (shockwaveProjectile, shockwaveTransform.position, Quaternion.Euler (Vector3.zero));
		newShockwave.GetComponent<ShockwaveProjectile> ().SetupProjectile (shockwaveDamage, shockwaveSpeed, shockwaveLifespan, facingVector, null);
	}

}
