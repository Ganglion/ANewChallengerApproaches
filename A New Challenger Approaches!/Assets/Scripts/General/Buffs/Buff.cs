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

public class Buff {

    // Buff attributes
    protected string buffName;
    protected float buffDuration;
    //protected BuffType buffType;
    //protected float buffValue;
    protected GameObject buffEffect;
    protected bool isStackable;
    protected int sourcePlayer;
    protected float buffTimestamp;

    public string BuffName { get { return buffName; } }
    public float BuffDuration { get { return buffDuration; } set { buffDuration = value; } }
    //public BuffType BuffType { get { return buffType; } }
    //public float BuffValue { get { return buffValue; } }
    public GameObject BuffEffect { get { return buffEffect; } }
    public bool IsStackable { get { return isStackable; } }
    public int SourcePlayer { get { return sourcePlayer; } }
    public float BuffTimestamp { get { return buffTimestamp; } set { buffTimestamp = value; } }

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

    public override bool Equals(object other) {
        Buff otherBuff = (Buff) other;
        bool hasSameName = this.buffName == otherBuff.buffName;
        bool hasSameSource = this.sourcePlayer == otherBuff.sourcePlayer;
        return hasSameName && hasSameSource;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public virtual void ExecuteBuff(UnitAttributes characterAttributes) {

    }
}
