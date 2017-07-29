using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes : MonoBehaviour {

    protected const float airborneAccelerationTimeMultiplier = 8;

    // Base attributes
    [SerializeField]
    protected float baseMaxHealth;
    [SerializeField]
    protected float baseMovementSpeed;
    [SerializeField]
    protected float timeTakenToReachMaxSpeed;
    [SerializeField]
    protected float baseJumpHeight;
    protected float baseDamageTakenFactor = 1;
    protected float baseDamageOutputFactor = 1;

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
    public bool CanMove { get { return canMove; } }
    public bool CanExecuteActions { get { return canExecuteActions; } }

    // Hidden attributes
    protected float movementSpeedMultiplier = 1;
    protected float jumpHeightMultiplier = 1;
    protected float damageTakenFactorMultiplier = 1;
    protected float damageOutputFactorMultiplier = 1;

    // Health system
    public virtual void ApplyAttack(float damageDealt) {
        TakeDamage(damageDealt);
    }

    public virtual void ApplyAttack(float damageDealt, params Buff[] attackBuffs) {
        ApplyAttack(damageDealt);
        for (int i = 0; i < attackBuffs.Length; i++) {
            ApplyBuff(attackBuffs[i]);
        }
    }

    protected void TakeDamage(float damageDealt) {
        if (currentHealth > 0) {
            currentHealth -= damageDealt;
            currentHealth = Mathf.Clamp(currentHealth, 0, baseMaxHealth);
        } else {
            Death();
        }
    }

    protected virtual void Death() {
        // Transform to orb
    }

    // Buff system
    protected List<Buff> buffList;

    protected void Awake() {
        buffList = new List<Buff>();
        ResetCurrentAttributes();
    }

    protected void Update() {
        ResetCurrentAttributes();
        for (int i = 0; i < buffList.Count; i++) {
            Buff currentBuff = buffList[i];
            if (currentBuff.IsExpired) {
                buffList.RemoveAt(i);
            } else {
                ParseBuff(currentBuff);
            }
        }
    }

    protected void ResetCurrentAttributes() {
        currentHealth = baseMaxHealth;
        currentMovementSpeed = baseMovementSpeed;
        currentJumpHeight = baseJumpHeight;
        currentDamageTakenFactor = baseDamageTakenFactor;
        currentDamageOutputFactor = baseDamageOutputFactor;
        currentGroundAcceleration = currentMovementSpeed / timeTakenToReachMaxSpeed;
        currentAirborneAcceleraion = currentGroundAcceleration / airborneAccelerationTimeMultiplier;
    }

    protected void ParseBuff(Buff buff) {
        switch (buff.BuffType) {
            case BuffType.MovementSpeedBuff:
                currentMovementSpeed *= buff.BuffValue;
                break;
            case BuffType.JumpHeightBuff:
                currentJumpHeight *= buff.BuffValue;
                break;
            case BuffType.DamageTakenBuff:
                currentDamageTakenFactor *= buff.BuffValue;
                break;
            case BuffType.DamageOutputBuff:
                currentDamageOutputFactor *= buff.BuffValue;
                break;
            case BuffType.DamagePerSecondBuff:
                this.ApplyAttack(buff.BuffValue * Time.deltaTime);
                break;
            case BuffType.RootBuff:
                canMove = false;
                break;
            case BuffType.DisarmBuff:
                canExecuteActions = false;
                break;
            case BuffType.StunBuff:
                canMove = false;
                canExecuteActions = false;
                break;
            default:
                break;
        }
    }

    protected void ApplyBuff(Buff newBuff) {
        if (newBuff.IsStackable) {
            buffList.Add(newBuff);
        } else {
            for (int i = 0; i < buffList.Count; i++) {
                Buff currentBuff = buffList[i];
                if (currentBuff.Equals(newBuff)) {
                    currentBuff.BuffDuration = newBuff.BuffDuration;
                    return;
                }
            }
            buffList.Add(newBuff);
        }
    }

}
