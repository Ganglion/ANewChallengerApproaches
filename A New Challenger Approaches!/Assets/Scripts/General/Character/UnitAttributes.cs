using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes : MonoBehaviour {

    // Constants
    protected Color defaultDamageColor;
    protected const float airborneAccelerationTimeMultiplier = 8;
    protected const float DAMAGE_OVER_TIME_INTERVAL = 0.25f;

    // Base attributes
    [SerializeField]
    protected float baseMaxHealth;
    [SerializeField]
    protected float baseMovementSpeed;
    [SerializeField]
    protected float timeTakenToReachMaxSpeed;
    [SerializeField]
    protected float baseJumpHeight;
    [SerializeField]
    protected float baseDamageTakenFactor = 1;
    [SerializeField]
    protected float baseDamageOutputFactor = 1;
    public float BaseMaxHealth { get { return baseMaxHealth; } }

    // Runtime attributes
    protected float currentHealth;
    protected float currentMovementSpeed;
    protected float currentGroundAcceleration;
    protected float currentAirborneAcceleraion;
    protected float currentJumpHeight;
    protected float currentDamageTakenFactor;
    protected float currentDamageOutputFactor;
    protected bool canMove = true;
    protected bool canExecuteActions = true;
    public float CurrentHealth { get { return currentHealth; } }
    public float CurrentDamageOutputFactor { get { return currentDamageOutputFactor; } }
    public float CurrentMovementSpeed { get { return currentMovementSpeed; } }
    public float CurrentGroundAcceleration { get { return currentGroundAcceleration; } }
    public float CurrentAirborneAcceleration { get { return currentAirborneAcceleraion; } }
    public float CurrentJumpHeight { get { return currentJumpHeight; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanExecuteActions { get { return canExecuteActions; } set { canExecuteActions = value; } }

    // Hidden attributes
    protected float movementSpeedMultiplier = 1;
    protected float jumpHeightMultiplier = 1;
    protected float damageTakenFactorMultiplier = 1;
    protected float damageOutputFactorMultiplier = 1;
    protected float damagePerSecond = 0;

    public float MovementSpeedMultiplier { get { return movementSpeedMultiplier; } set { movementSpeedMultiplier = value; } }
    public float JumpHeightMultiplier { get { return jumpHeightMultiplier; } set { jumpHeightMultiplier = value; } }
    public float DamageTakenFactorMultiplier { get { return damageTakenFactorMultiplier; } set { damageTakenFactorMultiplier = value; } }
    public float DamageOutputFactorMultiplier { get { return damageOutputFactorMultiplier; } set { damageOutputFactorMultiplier = value; } }
    public float DamagePerSecond { get { return damagePerSecond; } set { damagePerSecond = value; } }

    // Runtime variables
    protected float currentDamageOverTimeIntervalCooldown = 0;
	protected bool isDead = false;

    protected virtual void Awake() {
        buffList = new List<Buff>();
        defaultDamageColor = Color.white;
        InitialiseCurrentAttributes();
    }

	// Health system (and a million overloaded methods)
	public void ApplyAttack(float damageDealt) {
		ApplyAttack (damageDealt, transform.position, defaultDamageColor, null);
	}

	public void ApplyAttack(float damageDealt, params Buff[] attackBuffs) {
		ApplyAttack (damageDealt, transform.position, defaultDamageColor, attackBuffs);
	}

    public void ApplyAttack(float damageDealt, Vector2 pointOfHit) {
		ApplyAttack(damageDealt, pointOfHit, defaultDamageColor, null);
    }

    public void ApplyAttack(float damageDealt, Vector2 pointOfHit, Color damageColor) {
		ApplyAttack(damageDealt, pointOfHit, damageColor, null);
    }

    public void ApplyAttack(float damageDealt, Vector2 pointOfHit, params Buff[] attackBuffs) {
		ApplyAttack(damageDealt, pointOfHit, defaultDamageColor, attackBuffs);
    }

    public virtual void ApplyAttack(float damageDealt, Vector2 pointOfHit, Color damageColor, params Buff[] attackBuffs) {
		TakeDamage(damageDealt, pointOfHit, damageColor, false);

        if (attackBuffs != null) {
            for (int i = 0; i < attackBuffs.Length; i++) {
                ApplyBuff(attackBuffs[i]);
            }
        }
    }

    protected void TakeDamage(float damageDealt, Vector2 point, Color damageColor, bool isDamageOverTime) {
        if (damageDealt < 0) {
            Debug.LogError("Damage dealt (" + damageDealt + ") is negative. Use the Heal method to restore health.");
            return;
        }
        if (currentHealth > 0 && damageDealt > 0) {
            if (!isDamageOverTime || currentDamageOverTimeIntervalCooldown <= 0) {
                DamageTextManager.Instance.SpawnDamageText(damageDealt, point, damageColor);
                if (isDamageOverTime) {
                    currentDamageOverTimeIntervalCooldown = DAMAGE_OVER_TIME_INTERVAL;
                }
            }
            currentHealth -= damageDealt;
            currentHealth = Mathf.Clamp(currentHealth, 0, baseMaxHealth);
        }
        if (!isDead && currentHealth <= 0) {
			isDead = true;
            Death();
        }
    }

	public void Heal(float healAmount) {
		if (healAmount < 0) {
			Debug.LogError("Heal amount (" + healAmount + ") is negative. Use the ApplyAttack method to deal damage.");
			return;
		}
		if (currentHealth > 0 && healAmount > 0) {
			currentHealth += healAmount;
			currentHealth = Mathf.Clamp(currentHealth, 0, baseMaxHealth);
		}
	}

    protected virtual void Death() {
        // Transform to orb
    }

    // Buff system
    protected List<Buff> buffList;

    protected void Update() {
        ResetCurrentAttributes();
        for (int i = 0; i < buffList.Count; i++) {
            Buff currentBuff = buffList[i];
            if (Time.time >= currentBuff.BuffTimestamp + currentBuff.BuffDuration) {
				RemoveBuffEffect (currentBuff);
                buffList.RemoveAt(i);
            } else {
                currentBuff.ExecuteBuff(this);
            }
        }

        TakeDamage(damagePerSecond * currentDamageTakenFactor * Time.deltaTime, transform.position, defaultDamageColor, true);
        currentDamageOverTimeIntervalCooldown -= Time.deltaTime;
    }

    protected void InitialiseCurrentAttributes() {
        currentHealth = baseMaxHealth;
        currentMovementSpeed = baseMovementSpeed;
        currentJumpHeight = baseJumpHeight;
        currentDamageTakenFactor = baseDamageTakenFactor;
        currentDamageOutputFactor = baseDamageOutputFactor;
        currentGroundAcceleration = currentMovementSpeed / timeTakenToReachMaxSpeed;
        currentAirborneAcceleraion = currentGroundAcceleration / airborneAccelerationTimeMultiplier;
    }

    protected void ResetCurrentAttributes() {
        currentMovementSpeed = baseMovementSpeed;
        currentJumpHeight = baseJumpHeight;
        currentDamageTakenFactor = baseDamageTakenFactor;
        currentDamageOutputFactor = baseDamageOutputFactor;
        currentGroundAcceleration = currentMovementSpeed / timeTakenToReachMaxSpeed;
        currentAirborneAcceleraion = currentGroundAcceleration / airborneAccelerationTimeMultiplier;
		movementSpeedMultiplier = 1;
		jumpHeightMultiplier = 1;
		damageTakenFactorMultiplier = 1;
		damageOutputFactorMultiplier = 1;
		damagePerSecond = 0;
    }

    protected void ApplyBuff(Buff newBuff) {
        if (newBuff.IsStackable) {
            newBuff.BuffTimestamp = Time.time;
            buffList.Add(newBuff);
			ApplyBuffEffect (newBuff);
        } else {
            for (int i = 0; i < buffList.Count; i++) {
                Buff currentBuff = buffList[i];
                if (currentBuff.Equals(newBuff)) {
                    currentBuff.BuffDuration = newBuff.BuffDuration;
                    return;
                }
            }
            newBuff.BuffTimestamp = Time.time;
            buffList.Add(newBuff);
			ApplyBuffEffect (newBuff);
        }
    }

	protected void ApplyBuffEffect(Buff buff) {
		if (buff.BuffEffect != null) {
			buff.BuffEffectObject = (GameObject) Instantiate (buff.BuffEffect, transform.position, Quaternion.Euler (Vector3.zero));
			//buff.BuffEffectObject.transform.localScale = transform.localScale;
			buff.BuffEffectObject.transform.parent = transform;
		}
	}

	protected void RemoveBuffEffect(Buff buff) {
		if (buff.BuffEffectObject != null) {
			GameObject tempBuffObject = buff.BuffEffectObject;
			buff.BuffEffectObject = null;
			Destroy (tempBuffObject);
		}
	}

}
