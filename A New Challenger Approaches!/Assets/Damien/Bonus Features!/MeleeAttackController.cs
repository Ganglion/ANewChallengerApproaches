using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour {

    //NOTE: For attacks in general, can make an 'impact' effect on hit as well, for both melee and ranged attacks

    // Target objects
    public LayerMask entitiesMask;

    // Objects
    [SerializeField]
    private Transform firingIndicator;

    [Header("Properties")]
    public float damage; // flat damage value
    public float range; // how far the ray casts to detect enemies
    public float knockbackForce; // either force or distance
    public float cooldown;

    [Header("Behaviour")]
    public bool splash; // if true, hits all possible enemies hit by cast, else damage the closest.

    [Header("Working variables")]
    public int attackIndex = 0; // the index of the sprite used
    public bool canAttack; 

    //[Header("Yet to implement")]
    //public float comboResetTiming = 0.4f;
    //public List<Sprite> attackSprites; // main idea is to iterate through this list to give 'combo-like' atacks
    //public List<float> damageList; // TO-DO: diff damage values for diff hits? 

    


    void Awake()
    {
        //attributes = GetComponent<UnitAttributes>();
        firingIndicator = transform.Find("FacingIndicator");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
	}

    void Attack()
    {
        Debug.DrawLine(transform.position, new Vector3(firingIndicator.position.x + Mathf.Sign(firingIndicator.position.x - transform.position.x)*range, firingIndicator.position.y, firingIndicator.position.z), Color.green, 1f);
        if (splash)
        {
            // hits all in the melee attack's range
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(transform.position, firingIndicator.position - transform.position, range + Vector2.Distance(firingIndicator.position, transform.position), entitiesMask);

            for (int i = 0; i < hits.Length; i++)
            {
                GameObject go = hits[i].transform.gameObject;
                // TO-DO: diff damage values
                DamageEnemyGameObject(go);
            }
        }
        else // single hit, probably nearest object
        { 
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, firingIndicator.position - transform.position, range + Vector2.Distance(firingIndicator.position, transform.position), entitiesMask);
            if (hit)
            {
                GameObject go = hit.transform.gameObject;
                DamageEnemyGameObject(go);
            }
        }
    }

    void DamageEnemyGameObject(GameObject go)
    {
        UnitAttributes attributes = go.GetComponent<UnitAttributes>();
        attributes.ApplyAttack(damage, transform.position, Color.blue);
    }
}
