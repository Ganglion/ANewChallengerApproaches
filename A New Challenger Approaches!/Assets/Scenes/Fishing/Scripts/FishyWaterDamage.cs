using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishyWaterDamage : MonoBehaviour {

    private const string PLAYER_LAYER = "Player";

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(PLAYER_LAYER)) {
            other.GetComponent<UnitAttributes>().ApplyAttack(3 * Time.deltaTime, other.transform.position);
        }
    }

}
