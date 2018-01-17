using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawningController : MonoBehaviour {

    public UnitAttributes player;
    public Transform facingIndicator;

    [SerializeField]
    KeyCode portal1Key = KeyCode.A;
    [SerializeField]
    KeyCode portal2Key = KeyCode.D;


    [SerializeField]
    float portalCooldown;

    // keeping track of cooldown
    float portal1runningTime;
    float portal2runningTime;

    [SerializeField]
    float range; 

    public GameObject portal1Prefab;
    public GameObject portal2Prefab;

    public PortalController portal1;
    public PortalController portal2;


    public float portalUptime = 10f;

    void Awake(){
        player = GetComponent<UnitAttributes>();
        facingIndicator = transform.Find("FacingIndicator");
        portal1runningTime = portalCooldown;
        portal2runningTime = portalCooldown;
    }

    void spawnPortal(int portalNumber, Vector3 pos){
        int direction = (facingIndicator.position.x - transform.position.x >= 0)? 1:-1;
        switch (portalNumber){
            case 1:
                if (portal1)
                {
                    Destroy(portal1.gameObject);
                    portal2.nextPortal = null;
                }
                portal1 = Instantiate(portal1Prefab, new Vector3(pos.x + direction*range, pos.y, pos.z), portal1Prefab.transform.rotation).GetComponent<PortalController>();
                portal1.uptime = portalUptime;
                if (portal2 && portal1){
                    portal2.anim.Play("PortalOpened");
                    portal1.anim.Play("PortalOpened");
                    portal2.nextPortal = portal1;
                    portal1.nextPortal = portal2;
                }
                portal1runningTime = 0;
                break;
            case 2:
                if (portal2)
                {
                    Destroy(portal2.gameObject);
                    portal1.nextPortal = null;
                }
                portal2 = Instantiate(portal2Prefab, new Vector3(pos.x + direction * range, pos.y, pos.z), portal1Prefab.transform.rotation).GetComponent<PortalController>();
                portal2.uptime = portalUptime;
                if (portal2 && portal1)
                {
                    portal2.anim.Play("PortalOpened");
                    portal1.anim.Play("PortalOpened");
                    portal2.nextPortal = portal1;
                    portal1.nextPortal = portal2;
                }
                portal2runningTime = 0;
                break;
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        portal1runningTime += Time.deltaTime;
        portal2runningTime += Time.deltaTime;
        if (Input.GetKeyDown(portal2Key) && portal2runningTime >= portalCooldown){
            spawnPortal(2, facingIndicator.position);
        }
        if (Input.GetKeyDown(portal1Key) && portal1runningTime >= portalCooldown){
            spawnPortal(1, facingIndicator.position);
        }
	}
}
