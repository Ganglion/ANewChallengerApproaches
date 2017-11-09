using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour {

    protected const string ENEMY_LAYER = "Enemy";
    protected BoxCollider2D playerCollider;
    protected UnitAttributes unitAttributes;
    public float damageCooldownTime;
    public float damageTaken;
    protected float currentHealth;
    protected float time;

    //gets components from the player and sets current health
    protected void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        unitAttributes = GetComponent<UnitAttributes>();
        damageCooldownTime = 0.3f;
    }

    //destroys player on contact with enemy 
    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        GameObject hitObject = other.gameObject;
        if (Time.time > time + damageCooldownTime)
        {
            if (hitObject.layer == LayerMask.NameToLayer(ENEMY_LAYER))
            {
                unitAttributes.ApplyAttack(damageTaken, new Vector2(0, 0));
                time = Time.time;
            }
        }
    }

    // Update is called once per frame
    protected void Update () {
        currentHealth = unitAttributes.CurrentHealth;
    }
}
