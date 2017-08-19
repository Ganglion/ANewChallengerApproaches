using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttributes : UnitAttributes {

    private UnitAttributes bridgeguardAttributes;

    protected override void Awake() {
        base.Awake();
        bridgeguardAttributes = transform.root.GetComponent<UnitAttributes>();
    }

    public override void ApplyAttack(float damageDealt, Vector2 point, Color damageColor, params Buff[] attackBuffs) {
        bridgeguardAttributes.ApplyAttack(damageDealt * currentDamageTakenFactor, point, damageColor, attackBuffs);
    }
}
