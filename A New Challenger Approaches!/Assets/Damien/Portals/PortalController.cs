using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    public bool allowed;

    public PortalController nextPortal;

    public List<string> tagsAllowed;

    public Animator anim;

    float runningTime;

    public float uptime = 5f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start(){
        runningTime = 0;
    }


    void OnTriggerEnter2D (Collider2D other)
    {
        
        if (tagsAllowed.Contains(other.gameObject.tag) && allowed == true)
        {
            allowed = false;
            nextPortal.allowed = false;

            other.transform.position = nextPortal.transform.position;
        }
    }

    void OnTriggerExit2D( Collider2D other)
    {
        allowed = true;
    }
	
	// Update is called once per frame
	void Update () {
        runningTime += Time.deltaTime;
        if (runningTime >= uptime){
            if (nextPortal)
            {
                this.nextPortal.anim.Play("PortalIdle");
                this.nextPortal.nextPortal = null;
            }
            Destroy(this.gameObject);
        }
	}
}
