using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireFlamesDamage : MonoBehaviour {

    private const string PLAYER_LAYER = "Player";

    private Buff[] spitfireBuffs;

    public void InitialiseSpitfireFlames(params Buff[] buffs) {
        spitfireBuffs = buffs;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(PLAYER_LAYER)) {
            other.GetComponent<UnitAttributes>().ApplyAttack(0, other.transform.position, spitfireBuffs);
        }
    }

}
