using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOrb : MonoBehaviour {

	private GameObject deadObject;
	private float count = 2.5f;

	public void SetupOrb(GameObject deathObject) {
		deadObject = deathObject;
	}

	void OnTriggerStay2D(Collider2D other) {
        Debug.Log("MAMAMAMAMA");
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player") && other.gameObject.activeInHierarchy) {
			count -= Time.deltaTime;
            Debug.Log(count);
			if (count <= 0) {
				UnitAttributes healObj = deadObject.GetComponent<UnitAttributes> ();
				healObj.Respawn();
				healObj.gameObject.SetActive (true);
				Destroy (this.gameObject);
			}
		}
	}


}
