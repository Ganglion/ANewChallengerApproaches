using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOrbMovement : ObjectMovement {

	void Update () {
        Move(new Vector2(0, -Time.deltaTime), false);
	}
}
