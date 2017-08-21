using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAnimationController : MonoBehaviour {

    // Components
    private CharacterInput objectMovement;
    private Animator objectAnimator;

    private void Awake() {
        objectMovement = GetComponent<CharacterInput>();
        objectAnimator = GetComponent<Animator>();
    }

    private void Update() {
        if (objectMovement.IsFacingRight) {
            objectAnimator.SetTrigger("FaceRight");
        } else {
            objectAnimator.SetTrigger("FaceLeft");
        }
    }


}
