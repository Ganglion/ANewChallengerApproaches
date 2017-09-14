using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectMovement))]
public class UnitInput : MonoBehaviour {

	// Runtime variables
	protected Vector2 currentVelocity;
	public Vector2 CurrentVelocity { get { return currentVelocity; } set { currentVelocity = value; } }

	// Components
	protected UnitAttributes characterAttributes;
	protected ObjectMovement characterMovement;

	protected virtual void Awake() {
		characterAttributes = GetComponent<UnitAttributes>();
		characterMovement = GetComponent<ObjectMovement> ();
	}

}
