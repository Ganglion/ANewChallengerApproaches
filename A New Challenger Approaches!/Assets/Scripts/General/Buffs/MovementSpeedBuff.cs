using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedBuff : Buff {

    protected float movementSpeedMultiplier;

    public override void ExecuteBuff(UnitAttributes characterAttributes) {
        characterAttributes.MovementSpeedMultiplier *= movementSpeedMultiplier;
    }

    public MovementSpeedBuff(float multiplier, string name, float duration, GameObject effect, bool stackable, int player = 0) {
        movementSpeedMultiplier = multiplier;
        buffName = name;
        buffDuration = duration;
        buffEffect = effect;
        isStackable = stackable;
        sourcePlayer = player;
    }

}
