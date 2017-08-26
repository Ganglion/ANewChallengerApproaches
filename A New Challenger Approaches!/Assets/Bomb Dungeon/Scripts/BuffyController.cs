using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffyController : UnitInput
{

    [SerializeField]
    protected Transform targetCharacter;
    [SerializeField]
    protected GameObject bomb;
    [SerializeField]
    protected GameObject bombLauncher;


    //actions
    private string ACTION_IDLE = "Buffy Idle";
    private float TIME_IDLE = 3;


    private string ACTION_WALK = "Buffy Walk";
    private float TIME_WALK = 1;

    private string ACTION_ATTACK = "Buffy Attack";
    private float TIME_ATTACK = 1;
    private float COOLDOWN_ATTACK = 6;
    private float attackCooldown = 0;

   
    protected Animator characterAnimator;
    protected float jumpHeight = 2;
    protected float movementSpeed = 7;
    protected float currentAcceleration = 1;
    protected float cooldownToNextAction = 0;

    // Runtime variables
    protected float initialScale;
    protected bool isDoingAction = false;
    protected string action;

    protected override void Awake()
    {
        base.Awake();
        characterAnimator = GetComponent<Animator>();
        initialScale = transform.localScale.x;
        action = ACTION_IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (!characterMovement.collisions.below)
        { // Is not on ground?
            currentVelocity.y += Time.deltaTime * -9.81f * 5; // Fall
        }
        else
        {
            currentVelocity.y = -0.1f;
        }

        //handle cooldowns
        cooldownToNextAction -= Time.deltaTime;
        attackCooldown -= Time.deltaTime;

        if(cooldownToNextAction <= 0 && characterAttributes.CanExecuteActions)
        {
            characterAnimator.SetBool("isMoving", false);
            characterAnimator.SetBool("isAttacking", false);
            //Set New Action
            //is enemy nearby?
            if (Vector2.Distance(targetCharacter.transform.position, transform.position) < 4)
            {
                //escape
               
                action = ACTION_WALK;
                characterAnimator.SetBool("isMoving", true);
                cooldownToNextAction = TIME_WALK;
            } else if(attackCooldown <= 0) {
                //attack
                //throw bombs everywhere
                action = ACTION_ATTACK;
                characterAnimator.SetBool("isAttacking", true);
                
                attackCooldown = COOLDOWN_ATTACK;
                cooldownToNextAction = TIME_ATTACK;
                StartCoroutine(LaunchFiveBombs());
            } else {
                action = ACTION_IDLE;
                cooldownToNextAction = TIME_IDLE;

            }

            Debug.Log("action playing: " + action);
        }
        

        //Execute Updates
        if(action == ACTION_IDLE)
        {
            currentVelocity.x = 0;
        } else if (action == ACTION_ATTACK)
        {
            currentVelocity.x = 0;
        } else if (action == ACTION_WALK){
            currentVelocity.x = movementSpeed * FaceDirectionAwayFromTarget();

        } else {
            Debug.Log("unknown action:" + action);
        }

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime);
        characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
        
    }


    protected IEnumerator LaunchFiveBombs()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.2f);
        LaunchBomb();
        yield return new WaitForSeconds(0.2f);
        LaunchBomb();
        yield return new WaitForSeconds(0.2f);
        LaunchBomb();
        yield return new WaitForSeconds(0.2f);
        LaunchBomb();
        yield return new WaitForSeconds(0.2f);
        LaunchBomb();
    }

    protected void LaunchBomb()
    {
        float bombGravity = 1;
        float bombSpeed = 10;

        //generate random direction upwards
        int randomAngleDeg = Random.Range(55, 95);
        float randomAngleRad = Mathf.Deg2Rad * randomAngleDeg;
        float y = Mathf.Sin(randomAngleRad) * bombSpeed;
        float x = Mathf.Cos(randomAngleRad) * FaceDirectionToTarget() * bombSpeed;


        //Debug.Log(horizontalSpeed + " " + verticalSpeed);
        GameObject newBomb = (GameObject)Instantiate(bomb, bombLauncher.transform.position, Quaternion.Euler(Vector3.zero));
        newBomb.GetComponent<Bomb>().SetupProjectile(1000, new Vector2(x, y), Random.Range(10,20), bombGravity, null);

    }

    protected int FaceDirectionToTarget()
    {
        //Debug.Log (targetCharacter.position.x - transform.position.x);
        bool targetIsToTheRight = ((targetCharacter.position.x - transform.position.x) > 0) ? true : false;
        if (targetIsToTheRight)
        {
            Vector3 transformScale = transform.localScale;
            transformScale.x = -initialScale;
            transform.localScale = transformScale;
            return 1;
        }
        else
        {
            Vector3 transformScale = transform.localScale;
            transformScale.x = initialScale;
            transform.localScale = transformScale;
            return -1;
        }
    }

    protected int FaceDirectionAwayFromTarget()
    {
        //Debug.Log (targetCharacter.position.x - transform.position.x);
        bool targetIsToTheRight = ((targetCharacter.position.x - transform.position.x) > 0) ? true : false;
        if (targetIsToTheRight)
        {
            Vector3 transformScale = transform.localScale;
            transformScale.x = initialScale;
            transform.localScale = transformScale;
            return -1;
        }
        else
        {
            Vector3 transformScale = transform.localScale;
            transformScale.x = -initialScale;
            transform.localScale = transformScale;
            return 1;
        }
    }

}
