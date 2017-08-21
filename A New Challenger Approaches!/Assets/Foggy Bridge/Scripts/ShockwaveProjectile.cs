using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : LinearProjectile {

	protected override void MoveProjectile () {
		CameraController camera = CameraController.Instance;
		float distanceFromCamera = (transform.position - camera.CameraTransform.position).magnitude;
		CameraController.Instance.ShakeCamera (.033f, 1f);
		base.MoveProjectile ();
	}

	protected override void OnHitPlayer (GameObject hitObject) {
		hitObject.GetComponent<UnitAttributes> ().ApplyAttack (projectileDamage, hitObject.transform.position);
	}

	protected override void OnHitEnemy (GameObject hitObject) { }

	protected override void OnHitStructure (GameObject hitObject) { }

}
