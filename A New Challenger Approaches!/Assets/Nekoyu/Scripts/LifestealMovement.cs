using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealMovement : MonoBehaviour {

	private GameObject nekoyu;
	private Vector2 velocity;
	private float LukeSaver;

	private void Awake() {
		nekoyu = GameObject.Find ("Nekoyu");
		velocity = Random.insideUnitCircle * 10f;
		LukeSaver = Random.value;
	}

	private void Update() {
		if (LukeSaver > 0.2f) {//Adjust the threshold to determine the ratio of particles to destroy. Hopefully this improves performance of your game
			Destroy (this.gameObject);
		}

		Vector3 directionToNekoyu = (nekoyu.transform.position - transform.position).normalized;
		velocity = Vector2.MoveTowards (velocity, directionToNekoyu*5f, Time.deltaTime*50f);
		transform.position += (Vector3)velocity * Time.deltaTime * 2f;

		if ((nekoyu.transform.position - transform.position).magnitude < .5f) {
			Destroy (this.gameObject);
		}
	}
}
