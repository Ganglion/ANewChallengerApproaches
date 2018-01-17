using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishyController : UnitInput {

    // Constants
    protected const string ZAP_TRIGGER = "FishyZap";
    protected const string BUBBLEBEAM_TRIGGER = "FishyBubblebeam";
    protected const string SPLASH_TRIGGER = "FishySplash";
    protected const string PLAYER_LAYER = "Player";

    // References
    [SerializeField]
    protected List<Transform> targetCharacters;

    [Header("AI")]
    // Fields
    [SerializeField]
    protected int zapWeight;
    [SerializeField]
    protected int bubblebeamWeight;
    [SerializeField]
    protected int splashWeight;

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

    // Components
    protected Animator characterAnimator;

    protected override void Awake() {
        base.Awake();
        characterAnimator = GetComponent<Animator>();
        totalWeights = zapWeight + bubblebeamWeight + splashWeight;
        initialScale = transform.localScale.x;
        lazerbeamTarget = targetCharacters[Random.Range(0, targetCharacters.Count)];
    }
    protected void FixedUpdate() {
        if (!isDead) {
            cooldownToNextAction -= Time.deltaTime;

            if (!isDoingAction && cooldownToNextAction <= 0) {
                int chosenAction = Random.Range(0, totalWeights);
                if (chosenAction < zapWeight) {
                    characterAnimator.SetTrigger(ZAP_TRIGGER);

                } else if ((chosenAction - zapWeight) < bubblebeamWeight) {
                    characterAnimator.SetTrigger(BUBBLEBEAM_TRIGGER);

                } else if ((chosenAction - zapWeight - bubblebeamWeight) < splashWeight) {
                    characterAnimator.SetTrigger(SPLASH_TRIGGER);
                }

                isDoingAction = true;
                StartCoroutine(ApplyCooldownDelay());
            }
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
        cooldownToNextAction = Random.Range(minCoolown, maxCooldown);
    }

    [Header("Lazerbeam")]
    [SerializeField]
    private Transform fishyLurePivot;
    [SerializeField]
    private Transform fishyLureIndicator;
    [SerializeField]
    private GameObject lazerbeam;
    [SerializeField]
    private GameObject lazerbeamHitEffect;
    [SerializeField]
    private float lazerbeamDamage;
    [SerializeField]
    private float lazerbeamSpeed;
    [SerializeField]
    private float lazerbeamLifespan;

    private Vector2 normDirToLazerbeamTargetFromPivot;
    private Transform lazerbeamTarget;

    [SerializeField]
    private int numberOfLazerbeams;
    [SerializeField]
    private float intervalBetweenLazerbeams;

    private void Update () {
        normDirToLazerbeamTargetFromPivot = (lazerbeamTarget.position - fishyLurePivot.position).normalized;
        fishyLureIndicator.localPosition = normDirToLazerbeamTargetFromPivot * 0.8f;
    }

    public void Lazerbeam() {
        lazerbeamTarget = targetCharacters[Random.Range(0, targetCharacters.Count)];
        StartCoroutine(LazerbeamAttack());
    }

    private IEnumerator LazerbeamAttack() {
        for (int i = 0; i < numberOfLazerbeams; i++) {
            FireLazerbeam();
            yield return new WaitForSeconds(intervalBetweenLazerbeams);
        }
    }

    private void FireLazerbeam() {
        CameraController.Instance.ShakeCamera(.125f, .75f);
        GameObject newLazerbeam = Instantiate(lazerbeam, fishyLureIndicator.position, Quaternion.Euler(Vector3.zero));
        Debug.Log(lazerbeamSpeed + " " + normDirToLazerbeamTargetFromPivot);
        newLazerbeam.GetComponent<LazerbeamProjectile>().SetupProjectile(lazerbeamDamage, lazerbeamSpeed * normDirToLazerbeamTargetFromPivot, lazerbeamLifespan, lazerbeamHitEffect);
    }

    [Header("Bubblebeam")]
    [SerializeField]
    private Transform fishyMouthIndicator;
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private float bubblebeamDamage;
    [SerializeField]
    private float bubblebeamSpeed;
    [SerializeField]
    private float bubblebeamLifespan;
    [SerializeField]
    private int numberOfBubbles;
    [SerializeField]
    private float intervalBetweenBubbles;

    public void Bubblebeam() {
        lazerbeamTarget = targetCharacters[Random.Range(0, targetCharacters.Count)];
        StartCoroutine(BubblebeamAttack());
    }

    private IEnumerator BubblebeamAttack() {
        for (int i = 0; i < numberOfBubbles; i++) {
            int currentIndex = i % targetCharacters.Count;
            Vector3 targetVelocity = (targetCharacters[currentIndex].position - fishyMouthIndicator.position).normalized * bubblebeamSpeed;
            FireBubblebeam(targetVelocity);
            yield return new WaitForSeconds(intervalBetweenBubbles);
        }
    }

    private void FireBubblebeam(Vector2 bubblebeamVelocity) {
        CameraController.Instance.ShakeCamera(.125f, .75f);
        GameObject newBubblebeam = Instantiate(bubble, fishyMouthIndicator.position, Quaternion.Euler(Vector3.zero));
        newBubblebeam.GetComponent<BubbleProjectile>().SetupProjectile(bubblebeamDamage, bubblebeamVelocity, bubblebeamLifespan);
    }

}
