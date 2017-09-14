using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireFlamesAutoDestruction : ParticleSystemAutoDestruction {

	protected Collider2D spitfireFlamesCollider;

	protected override void Awake () {
		base.Awake ();
		spitfireFlamesCollider = GetComponent<Collider2D> ();
	}

	protected override void Update() {
		base.Update ();
		if (!ps.isEmitting) {
			spitfireFlamesCollider.enabled = false;
		}
	}

}
