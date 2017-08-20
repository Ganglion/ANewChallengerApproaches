using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeBuff : Buff {

    protected float damageOverTime;

    public override void ExecuteBuff(UnitAttributes characterAttributes) {
        characterAttributes.DamagePerSecond += damageOverTime;
    }

    public DamageOverTimeBuff(float damage, string name, float duration, GameObject effect, bool stackable, int player = 0) {
        damageOverTime = damage;
        buffName = name;
        buffDuration = duration;
        buffEffect = effect;
        isStackable = stackable;
        sourcePlayer = player;
    }

}
