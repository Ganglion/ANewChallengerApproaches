using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour {

    protected const int NUMBER_OF_DAMAGE_ANIMATIONS = 3;

    protected Animator damageAnimator;

    protected void Awake() {
        damageAnimator = GetComponent<Animator>();
    }

	protected void OnEnable() {
        damageAnimator.SetInteger("damageID", Random.Range(0, NUMBER_OF_DAMAGE_ANIMATIONS));
        StartCoroutine(RemoveDamageTextAfterAnimationCompletes());
	}

    protected IEnumerator RemoveDamageTextAfterAnimationCompletes() {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(damageAnimator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }
}
