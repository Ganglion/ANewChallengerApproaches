using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeguardAttributes : UnitAttributes {

	protected Animator bridgeguardAnimator;
	protected BridgeguardController bridgeguardController;

	protected override void Awake () {
		base.Awake ();
		bridgeguardAnimator = GetComponent<Animator> ();
		bridgeguardController = GetComponent<BridgeguardController> ();
	}

	protected override void Death () {
		bridgeguardController.SetDead ();
		bridgeguardAnimator.SetTrigger ("BridgeguardDeath");
	}
}
