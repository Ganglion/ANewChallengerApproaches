using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour {
	protected Animator characterAnimator;
	protected Rigidbody2D rb;
	protected bool inAir;
	// Use this for initialization
	void Start () {
		characterAnimator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.y > 0.2 || rb.velocity.y < -0.2) {
			inAir = true;
		} else {
			inAir = false;
		}

		if (inAir == true){
			characterAnimator.SetInteger ("Movement", 2);
		} else {
			characterAnimator.SetInteger ("Movement", 0);
		}

		characterAnimator.SetInteger ("Attack", 0);

		if (inAir == false) {
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) {
				characterAnimator.SetInteger ("Movement", 1);
			}
		}
		//Dash
		if (Input.GetKey (KeyCode.Space)) {
			characterAnimator.SetInteger ("Movement", 3);
		}

		//Slash
		if (Input.GetKeyDown (KeyCode.C)){
			//Placeholder code, attack animations must also take in account cooldown.
			characterAnimator.SetInteger ("Attack", 1);
		}
		//Jab
		if (Input.GetKey (KeyCode.X)) {
			characterAnimator.SetInteger ("Attack", 2);
		}
		//Lifesteal
		if (Input.GetKey (KeyCode.Z)) {
			characterAnimator.SetInteger ("Attack", 3);
		}
		//Spellswipe
		if (Input.GetKey (KeyCode.V)) {
			characterAnimator.SetInteger ("Attack", 4);
		}
		//Buff
		if (Input.GetKey (KeyCode.F)) {
			characterAnimator.SetInteger ("Attack", 5);
		}
					
	}
}
