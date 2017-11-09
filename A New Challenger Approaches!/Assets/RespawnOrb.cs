using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOrb : MonoBehaviour {

	private GameObject deadObject;
	private float count = 1;

	public void SetupOrb(GameObject deathObject) {
		deadObject = deathObject;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Player") && other.gameObject.activeInHierarchy) {
			count -= Time.deltaTime;
			if (count <= 0) {
				UnitAttributes healObj = deadObject.GetComponent<UnitAttributes> ();
				healObj.Respawn();
				healObj.gameObject.SetActive (true);
				Destroy (this.gameObject);
			}
		}
	}


}
