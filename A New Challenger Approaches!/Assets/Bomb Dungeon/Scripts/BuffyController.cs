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
    protected GameObject fireBomb;
    [SerializeField]
    protected GameObject laser;
    [SerializeField]
    protected GameObject bombLauncher;
    [SerializeField]
    protected Transform topBehind;
    [SerializeField]
    protected Transform topInfront;



    //actions
    private string ACTION_IDLE = "Buffy Idle";
    private float TIME_IDLE = 3;
    
    private string ACTION_WALK = "Buffy Walk";
    private float TIME_WALK = 1;

    private const string ACTION_BOMB_ATTACK = "Buffy Attack";
    private const float TIME_BOMB_ATTACK = 1;
    private const int NUMBER_OF_BOMBS = 10;
    private float COOLDOWN_BOMB_ATTACK = 6;
    private float bombAttackCooldown = 0;

    private const float TIME_FIRE_BOMB_ATTACK = 3;
    private const float TIME_FIRE_BOMB_CHARGE = 1;
    private const int NUMBER_OF_FIRE_BOMBS = 20;
    private float COOLDOWN_FIRE_BOMB_ATTACK = 6;
    private float fireBombAttackCooldown = 0;

    private const string ACTION_CHASE = "Buffy Chase";
    private const float TIME_CHASE = 2;

    //states
    private const string STATE_NORMAL = "Normal";
    private const string STATE_POWERED = "Powered";
    private const string STATE_TRANSITION = "Transition";
    private static float TIME_NORMAL_TO_POWERED = 3;
    protected static string currentState = STATE_NORMAL;

   
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
        handleGravity();
        handleCooldowns();
        handleBossState();
        handleBossActions();
        //Execute Updates
        if (action == ACTION_IDLE)
        {
            currentVelocity.x = 0;
        } else if (action == ACTION_BOMB_ATTACK)
        {
            currentVelocity.x = 0;
        } else if (action == ACTION_WALK) {
            currentVelocity.x = movementSpeed * FaceDirectionAwayFromTarget();
        } else if (action == ACTION_CHASE)
        {
            currentVelocity.x = 3 * FaceDirectionToTarget();
        } else {
            Debug.Log("unknown action:" + action);
        }

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime);
        characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
        
    }

    protected void handleGravity()
    {
        if (!characterMovement.collisions.below && currentState == STATE_NORMAL)
        { // Is not on ground?
            currentVelocity.y += Time.deltaTime * -9.81f * 5; // Fall
        }
        else if (currentState == STATE_TRANSITION)
        {
            currentVelocity.y += Time.deltaTime * 1.2f;
        } else
        {
            currentVelocity.y = 0f;
        }
    }

    protected void handleCooldowns()
    {
        cooldownToNextAction -= Time.deltaTime;
        bombAttackCooldown -= Time.deltaTime;
        fireBombAttackCooldown -= Time.deltaTime;
    }

    protected void handleBossState()
    {
        //can we change our state?
        if(cooldownToNextAction <= 0)
        {
            if (currentState == STATE_NORMAL && characterAttributes.CurrentHealth < characterAttributes.BaseMaxHealth / 2)
            {
                characterAnimator.SetBool("isMoving", false);
                characterAnimator.SetBool("isAttacking", false);
                currentState = STATE_TRANSITION;
                cooldownToNextAction = TIME_NORMAL_TO_POWERED;
                StartCoroutine(transitToPowered());
            }
        }
 
    }

    protected IEnumerator transitToPowered()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(3);
        currentState = STATE_POWERED;
    }

    protected void handleBossActions()
    {
        switch (currentState)
        {
            case STATE_NORMAL:
                handleNormalBossActions();
                return;
            case STATE_POWERED:
                handlePoweredBossActions();
                return;
            default:
                Debug.Log(currentState);
                break;

        }
           
    }

    protected void handleNormalBossActions()
    {
        if (cooldownToNextAction <= 0 && characterAttributes.CanExecuteActions)
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
            }
            else if (bombAttackCooldown <= 0)
            {
                //attack
                //throw bombs everywhere
                action = ACTION_BOMB_ATTACK;
                characterAnimator.SetBool("isAttacking", true);

                bombAttackCooldown = COOLDOWN_BOMB_ATTACK;
                cooldownToNextAction = TIME_BOMB_ATTACK;
                StartCoroutine(launchBombs());
                //
            }
            else if (Mathf.Abs(transform.position.x - targetCharacter.transform.position.x) > 8)
            {
                //chase
                action = ACTION_CHASE;
                cooldownToNextAction = TIME_CHASE;
                characterAnimator.SetBool("isMoving", true);
            }
            else
            {
                action = ACTION_IDLE;
                cooldownToNextAction = TIME_IDLE;
            }

            Debug.Log("action playing: " + action);
        }
    }

    protected void handlePoweredBossActions()
    {
        if (cooldownToNextAction <= 0 && characterAttributes.CanExecuteActions)
        {

            characterAnimator.SetBool("isMoving", false);
            characterAnimator.SetBool("isAttacking", false);
            if (fireBombAttackCooldown <= 0)
            {
                action = ACTION_BOMB_ATTACK;
                characterAnimator.SetBool("isAttacking", true);

                fireBombAttackCooldown = COOLDOWN_FIRE_BOMB_ATTACK;
                cooldownToNextAction = TIME_FIRE_BOMB_ATTACK;
                StartCoroutine(spawnLasers(20, 4));
                StartCoroutine(spawnRingOfFire(NUMBER_OF_FIRE_BOMBS, 2, TIME_FIRE_BOMB_CHARGE));
            } else if (bombAttackCooldown <= 0)
            {
                action = ACTION_BOMB_ATTACK;
                characterAnimator.SetBool("isAttacking", true);

                bombAttackCooldown = COOLDOWN_BOMB_ATTACK;
                cooldownToNextAction = TIME_BOMB_ATTACK;
                StartCoroutine(launchBombs());
                
            } else if (Mathf.Abs(transform.position.x - targetCharacter.transform.position.x) > 5)
            {
                //chase
                action = ACTION_CHASE;
                cooldownToNextAction = TIME_CHASE;                
            }
            else
            {
                action = ACTION_IDLE;
                cooldownToNextAction = TIME_IDLE;
            }
        }
    }

 


    /*
     * BOMB_ATTACK METHODS 
     */
    protected IEnumerator launchBombs()
    {
        yield return new WaitForEndOfFrame();

        float interval = TIME_BOMB_ATTACK / (NUMBER_OF_BOMBS-1);
        for(int i = 0; i < NUMBER_OF_BOMBS; i++)
        {
            LaunchBomb();
            yield return new WaitForSeconds(interval);
        }       
    }

    protected void LaunchBomb()
    {
        float bombGravity = 1;
        float bombSpeed = 15;

        //generate random direction upwards
        int randomAngleDeg = Random.Range(55, 95);
        float randomAngleRad = Mathf.Deg2Rad * randomAngleDeg;
        float y = Mathf.Sin(randomAngleRad) * bombSpeed;
        float x = Mathf.Cos(randomAngleRad) * FaceDirectionToTarget() * bombSpeed;


        //Debug.Log(horizontalSpeed + " " + verticalSpeed);
        GameObject newBomb = (GameObject)Instantiate(bomb, bombLauncher.transform.position, Quaternion.Euler(Vector3.zero));
        newBomb.GetComponent<Bomb>().SetupProjectile(5, new Vector2(x, y), Random.Range(10,20), bombGravity, null);
    }

    /*
    * FIRE_BOMB_ATTACK METHODS 
    */

    protected void launchFireBombInDirection(Vector2 direction)
    {
        float bombGravity = 0;
        float bombSpeed = 1;
        float bombAcceleration = 1;
        Vector2 normalizedDirection = direction.normalized;

        GameObject newFireBomb = (GameObject)Instantiate(fireBomb, bombLauncher.transform.position, Quaternion.Euler(Vector3.zero));
        newFireBomb.GetComponent<FireBomb>().SetupProjectile(5, normalizedDirection * bombSpeed, 10, bombGravity, bombAcceleration, null);
    }

    protected GameObject spawnFireBombAtPositionWithDirection(Vector2 position, Vector2 direction)
    {

        float bombGravity = 0;
        float bombSpeed = 5;
        float bombAcceleration = 5;
        Vector2 normalizedDirection = direction.normalized;
        GameObject newFireBomb = (GameObject)Instantiate(fireBomb, position, Quaternion.Euler(Vector3.zero));
        newFireBomb.GetComponent<FireBomb>().SetupProjectile(5, normalizedDirection * bombSpeed, 10, bombGravity, bombAcceleration, null);
        return newFireBomb;
    }

    protected IEnumerator spawnRingOfFire(int num_fireBomb, float distanceFromBoss, float timeToSpawn)
    {
        List<GameObject>fireBombs = new List<GameObject>();

        yield return new WaitForEndOfFrame();
        Vector2 initialDirection = new Vector2(FaceDirectionToTarget(), 0);
        Vector2 initialDisplacement = initialDirection * distanceFromBoss;
        float degrees = FaceDirectionToTarget() * 360f / num_fireBomb;
        float time = timeToSpawn / (num_fireBomb - 1);

        for (int i = 0; i < num_fireBomb; i++)
        {
            GameObject newFireBomb = spawnFireBombAtPositionWithDirection(initialDisplacement + (Vector2)bombLauncher.transform.position, initialDirection);
            fireBombs.Add(newFireBomb);
            initialDirection = Quaternion.Euler(0, 0, degrees) * initialDirection;
            initialDisplacement = Quaternion.Euler(0, 0, degrees) * initialDisplacement;
            yield return new WaitForSeconds(time);
        }

        foreach(GameObject fb in fireBombs)
        {
            if(fb != null)
                fb.GetComponent<FireBomb>().FireProjectile();
        }
    }

    /*
     * LASER_ATTACK METHODS
     */

    protected IEnumerator spawnLasers(int num_lasers, float timeToSpawn)
    {
        List<GameObject> lasers = new List<GameObject>();

        float topInfrontX = topInfront.position.x;
        float topBehindX = topBehind.position.x;
        float distance = Mathf.Abs(topInfrontX - topBehindX);
        float distanceBetweenLaser = distance / (num_lasers - 1);
        int directionOfSpawn = ((topInfrontX - topBehindX) > 0) ? -1 : 1;
        Vector2 spawnPosition = topInfront.position;
        float time = timeToSpawn / (num_lasers - 1);

        yield return new WaitForEndOfFrame();

        for(int i = 0; i < num_lasers; i++)
        {
            GameObject newLaser = (GameObject)Instantiate(laser, spawnPosition, Quaternion.Euler(Vector3.zero));
            //newLaser.GetComponent<Laser>().SpawnLaser(20, 5, 13, Vector2.down);
            spawnPosition += new Vector2(distanceBetweenLaser * directionOfSpawn, 0);
            yield return new WaitForSeconds(time);
        }

        foreach (GameObject spawnedLaser in lasers)
        {
            if (spawnedLaser != null)
                spawnedLaser.GetComponent<Laser>().FireLaser();
        }



    }

    protected void spawnLaserAtPosition(Vector2 position)
    {

    }


    /*
     * DIRECTION METHODS
     */

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
