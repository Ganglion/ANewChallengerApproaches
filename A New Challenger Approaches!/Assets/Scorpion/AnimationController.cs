using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	// Kinda hard to explain the whole animation thing, come to us for help with animations

    // Components
    private CharacterMovement objectMovement;
    private Animator objectAnimator;

    private void Awake() {
        objectMovement = GetComponent<CharacterMovement>();
        objectAnimator = GetComponent<Animator>();
    }

	private void Update() {
		objectAnimator.SetBool ("isFacingRight", objectMovement.IsFacingRight);
	}

}
