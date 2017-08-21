using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemAutoDestruction : MonoBehaviour {

	protected ParticleSystem ps;

	protected virtual void Awake() {
		ps = GetComponent<ParticleSystem> ();
	}

	protected virtual void Update() {
		if (!ps.IsAlive ()) {
			Destroy (gameObject);
		}
	}

}
