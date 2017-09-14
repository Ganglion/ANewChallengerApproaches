using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	protected GameObject buffEffectObject;

    public string BuffName { get { return buffName; } }
    public float BuffDuration { get { return buffDuration; } set { buffDuration = value; } }
    //public BuffType BuffType { get { return buffType; } }
    //public float BuffValue { get { return buffValue; } }
    public GameObject BuffEffect { get { return buffEffect; } }
    public bool IsStackable { get { return isStackable; } }
    public int SourcePlayer { get { return sourcePlayer; } }
    public float BuffTimestamp { get { return buffTimestamp; } set { buffTimestamp = value; } }
	public GameObject BuffEffectObject { get { return buffEffectObject; } set { buffEffectObject = value; } }

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
