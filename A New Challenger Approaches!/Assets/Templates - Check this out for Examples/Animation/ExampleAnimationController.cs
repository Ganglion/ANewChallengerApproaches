using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAnimationController : MonoBehaviour {

	// Kinda hard to explain the whole animation thing, come to us for help with animations

    // Components
    private ExampleCharacterMovement objectMovement;
    private Animator objectAnimator;

    private void Awake() {
        objectMovement = GetComponent<ExampleCharacterMovement>();
        objectAnimator = GetComponent<Animator>();
    }

	private void Update() {
		objectAnimator.SetBool ("isFacingRight", objectMovement.IsFacingRight);
	}

}
