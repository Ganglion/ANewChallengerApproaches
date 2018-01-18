using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishyAttributes : UnitAttributes {

    protected Animator fishyAnimator;
    protected FishyController fishyController;

    protected override void Awake() {
        base.Awake();
        fishyAnimator = GetComponent<Animator>();
        fishyController = GetComponent<FishyController>();
    }

    protected override void Death() {
        fishyController.SetDead();
        fishyAnimator.SetTrigger("FishyDeath");
    }
}
