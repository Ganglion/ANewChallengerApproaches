using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionAttribute : UnitAttributes {

	public float damageMultiplier = 1;
	public float upgradeHealthBuffer;
	public float timeBetweenBufferRefresh;
	public GameObject shieldObject;
	public float shieldFlashTime;
	protected SpriteRenderer spriteRenderer;
	public Sprite damageSprite;
	public Sprite normalSprite;
	public Sprite blockingSprite;

	public float damageSpriteDisplayDuration;

	//improvement!
	public GameObject improvementOrb;
	public GameObject mainEnemy;
	protected UnitAttributes enemyAttributes;
	protected float enemyMaxHealth;
	protected Animator animator;

	protected float damageSpriteCounter;

	protected MeshRenderer shieldRenderer;
	protected float currentHealthBuffer;
	protected float currentBufferTimer;
	protected bool isUpgraded;
	protected float timeToDisplayShield;

	// Use this for initialization
	void Start () {
		if(spriteRenderer == null) {
			spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		}
		normalSprite = spriteRenderer.sprite;
		
		currentBufferTimer = 0;
		currentHealthBuffer = 0;
		timeToDisplayShield = 0;
		isUpgraded = false;
		shieldRenderer = shieldObject.GetComponent<MeshRenderer>();
		shieldRenderer.enabled = false;
		if(mainEnemy == null) {
			//assign one.
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			//In general I've found that the bosses tend to be at the end of the array, so we're going to start from the end
			//and then pick the first one that matches the criteria.
			for(int i = enemies.Length - 1; i >= 0; i++) {
				if(enemies[i].GetComponent<UnitAttributes>() != null) {
					mainEnemy = enemies[i];
					break;
				}
			}
			if(mainEnemy == null) { //still not found
				//just pick one.
				mainEnemy = enemies[enemies.Length - 1];
			}
		}
		enemyAttributes = mainEnemy.GetComponent<UnitAttributes>();
		enemyMaxHealth = enemyAttributes.CurrentHealth;
		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyAttributes.CurrentHealth * 2 < enemyMaxHealth) {
			improvementOrb.GetComponent<UpgradeOrbBehavior>().isActive = true;
			enemyMaxHealth = 0;
		}
		if(isUpgraded) {
			currentBufferTimer += Time.deltaTime;
			if(currentBufferTimer >= timeBetweenBufferRefresh) {
				currentBufferTimer -= timeBetweenBufferRefresh;
				currentHealthBuffer = upgradeHealthBuffer;
				timeToDisplayShield = shieldFlashTime;
			}
		}
		if(timeToDisplayShield > 0) {
			timeToDisplayShield -= Time.deltaTime;
			if(timeToDisplayShield > 0) {
				shieldRenderer.enabled = true;
			} else {
				shieldRenderer.enabled = false;
			}
		}

		if(damageSpriteCounter > 0) {
			damageSpriteCounter -= Time.deltaTime;
			spriteRenderer.sprite = damageSprite;
			animator.enabled = false;
		} else if(damageMultiplier < 1) {
			spriteRenderer.sprite = blockingSprite;
			animator.enabled = false;
		} else if(damageMultiplier == 1) {
			spriteRenderer.sprite = normalSprite;
			animator.enabled = true;
		}
	}

	public override void ApplyAttack(float damageDealt, Vector2 pointOfHit, Color damageColor, params Buff[] attackBuffs) {
		float damageToTake = damageDealt * damageMultiplier;
		if(currentHealthBuffer > 0) {
			if(currentHealthBuffer > damageToTake) {
				currentHealthBuffer -= damageToTake;
				damageToTake = 0;
			} else {
				damageToTake -= currentHealthBuffer;
				currentHealthBuffer = 0;
			}
			timeToDisplayShield = shieldFlashTime;
		}
		TakeDamage(damageToTake, pointOfHit, damageColor, false);
		damageSpriteCounter = damageSpriteDisplayDuration;

        if (attackBuffs != null) {
            for (int i = 0; i < attackBuffs.Length; i++) {
                ApplyBuff(attackBuffs[i]);
            }
        }
    }

    public void gainDefences() {
    	isUpgraded = true;
    	currentHealthBuffer = upgradeHealthBuffer;
    	timeToDisplayShield = shieldFlashTime;
    }
}