using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour {
	//The lifesteal scripts spawns a fixed number of invisible lifesteal projectiles.
	//This script destroys them after the Z key is released, preventing subsequent firing of projectiles.

	// Update is called once per frame
	void Update () {
		if (!Input.GetKey (KeyCode.Z)) {
			StartCoroutine ("Delay");

		}
	}

	IEnumerator Delay(){
		yield return new WaitForSeconds (0.5f);
		Destroy (this.gameObject);
	}
}
