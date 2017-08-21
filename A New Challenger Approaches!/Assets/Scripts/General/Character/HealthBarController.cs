using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {

    // Constants
    private const string HEALTH_BAR_SCALER_NAME = "Hp Bar Scaler";
    private const string HEALTH_BAR_BUFFER_SCALER_NAME = "Hp Bar Buffer Scaler";

    // References
    [SerializeField]
    private UnitAttributes unitAttributes;

    // Fields
    [SerializeField]
    private float bufferSpeed = 0.5f;
    [SerializeField]
    private Vector2 healthBarOffset;

    // Components
    private Transform healthBarScaler;
    private Transform healthBarBufferScaler;

    private void Awake() {
        healthBarScaler = transform.Find(HEALTH_BAR_SCALER_NAME);
        healthBarBufferScaler = transform.Find(HEALTH_BAR_BUFFER_SCALER_NAME);
    }

    private void Update() {
		float fractionHealth = unitAttributes.CurrentHealth / unitAttributes.BaseMaxHealth;
        healthBarScaler.localScale = new Vector3(fractionHealth, 1, 1);

        if (healthBarBufferScaler.localScale.x > fractionHealth) {
            float nextBufferFraction = Mathf.MoveTowards(healthBarBufferScaler.localScale.x, fractionHealth, bufferSpeed * Time.deltaTime);
            healthBarBufferScaler.localScale = new Vector3(nextBufferFraction, 1, 1);
        }

        transform.position = unitAttributes.transform.position + (Vector3)healthBarOffset;
    }

}
