using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishyController : UnitInput {

	// Constants
	protected const string READY_TRIGGER = "FishyReady";
	protected const string LAZOR_TRIGGER = "FishyLazor";
	protected const string BUBBLEBEAM_TRIGGER = "FishyBubblebeam";
	protected const string PLAYER_LAYER = "Player";

	[Header("Delay")]
	// Delay
	[SerializeField]
	protected float minCoolown;
	[SerializeField]
	protected float maxCooldown;

	// Runtime variables
	protected bool hasEnteredCombat = false;
	protected bool isDoingAction = false;
	protected float cooldownToNextAction = 0;
	protected int totalWeights;
	protected float initialScale;
	protected bool isDead = false;

	// References
	[SerializeField]
	protected List<Transform> targetCharacters;
	[SerializeField]
	protected Transform fishyLure;
	[SerializeField]
	protected Transform fishyLureAim;

	[Header("Lazerbeam")]
	[SerializeField]
	protected int numberOfLaserbeams;
	[SerializeField]
	protected float laserbeamInterval;
	[SerializeField]
	protected float lazerbeamDamage;
	[SerializeField]
	protected float lazerbeamSpeed;
	[SerializeField]
	protected float indicatorRadius;
	protected Transform fishyLureTarget;

	// Components
	protected Animator characterAnimator;

	protected override void Awake() {
		base.Awake();
		characterAnimator = GetComponent<Animator>();
		//totalWeights = moveWeight + stabWeight + spitfireWeight;
		initialScale = transform.localScale.x;
		fishyLureTarget = targetCharacters [Random.Range (0, targetCharacters.Count)];
	}

	protected void Update() {
		Vector3 dirToTarget = (fishyLureTarget.position - fishyLure.position).normalized;
		fishyLureAim.localPosition = fishyLure.position + dirToTarget * indicatorRadius;
	}

	protected IEnumerator ApplyCooldownDelay() {
		isDoingAction = true;
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
		isDoingAction = false;
		cooldownToNextAction = Random.Range (minCoolown, maxCooldown);
	}

	public void PrepLazerbeam() {

	}

	public void Lazerbeam() {
		StartCoroutine (FireLazerbeams ());
	}

	protected IEnumerator FireLazerbeams() {
		for (int i = 0; i < numberOfLaserbeams; i++) {

		}
	}

}
