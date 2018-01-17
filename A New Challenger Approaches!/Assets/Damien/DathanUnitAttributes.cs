using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DathanUnitAttributes : UnitAttributes {

	//Multiple Jumps Attribute
	[Header("Multiple Jumps Attribute")]
	[SerializeField]
	public int maxJumps = 1;

	// Dash Attributes
	[SerializeField]
	public float baseDashDistance;
	//[SerializeField]
	//public float dashEnergyCost;
	[SerializeField]
	public float dashCooldown;
	[SerializeField]
	public float timeSinceLastDash = 99;

	[Header("Fly Settings")]
	//public float flyEnergyCost;
	public float flySpeed; // upwards MAX speed
	public float flyAccel;

}
