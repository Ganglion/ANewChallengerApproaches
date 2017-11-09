using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum BuffType {
    MovementSpeedBuff,
    JumpHeightBuff,
    DamageTakenBuff,
    DamageOutputBuff,
    DamagePerSecondBuff,
    RootBuff,
    DisarmBuff,
    StunBuff
}*/

public class DashBuff : Buff {

	public DashBuff(string name, float duration, GameObject effect, bool stackable, int player = 0) {
		buffName = name;
		buffDuration = duration;
		buffEffect = effect;
		isStackable = stackable;
		sourcePlayer = player;
	}

	/*
    // Runtime attributes
	private bool isExpired;
	public bool IsExpired { get { return isExpired; } }

	void Update () {
		Debug.Log(buffDuration);
		if (buffDuration > 0) {
			buffDuration -= Time.deltaTime;
		} else {
			isExpired = true;
		}
	}*/

	public override void ExecuteBuff(UnitAttributes characterAttributes) {
		characterAttributes.DamageTakenFactorMultiplier *= 0;
	}
}
