using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextManager : Singleton<DamageTextManager> {

    // Constants
    private const int INITIAL_POOL_SIZE = 5;
    private const int DAMAGE_TEXT_CHILD_INDEX = 0;

    // Prefabs
    [SerializeField]
    private GameObject damageTextPrefab;

    // Runtime variables
    private List<GameObject> damageTextPool;

	private void Awake() {
        damageTextPool = new List<GameObject>();
        for (int i = 0; i < INITIAL_POOL_SIZE; i++) {
            GameObject newDamageText = (GameObject)Instantiate(damageTextPrefab);
            damageTextPool.Add(newDamageText);
        }
	}

    public void SpawnDamageText(float damage, Vector2 position, Color damageColor) {
        GameObject newDamageText = GetDamageTextObject();
        Text damageText = newDamageText.transform.GetChild(DAMAGE_TEXT_CHILD_INDEX).GetComponent<Text>();
        damageText.color = damageColor;
        damageText.text = Mathf.Ceil(damage).ToString();
        newDamageText.transform.position = position;
        newDamageText.SetActive(true);
	}

    private GameObject GetDamageTextObject() {
        for (int i = 0; i < damageTextPool.Count; i++) {
            if (!damageTextPool[i].activeInHierarchy) {
                return damageTextPool[i];
            }
        }
        GameObject newDamageText = (GameObject)Instantiate(damageTextPrefab);
        damageTextPool.Add(newDamageText);
        return newDamageText;
    }
}
