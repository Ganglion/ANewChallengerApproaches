using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeoutController : MonoBehaviour {

    ExampleLinearProjectile projectile;

    [SerializeField]
    float runningTime = 0;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    float startFadeTime;

    [SerializeField]
    float endFadeTime;

    Coroutine fadeCoroutine = null;

    private void Awake()
    {
        projectile = GetComponent<ExampleLinearProjectile>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        runningTime = 0;

    }

    // Use this for initialization
    void Start () {
        endFadeTime = projectile.projectileLifespan; // made changes to expose projectile's projectileLifespan variable
	}
	
	// Update is called once per frame
	void Update () {
        runningTime += Time.deltaTime;
        if (runningTime >= startFadeTime && fadeCoroutine == null){
            fadeCoroutine = StartCoroutine(FadeOut(endFadeTime - startFadeTime));
        }

	}

    public void Refresh()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        runningTime = 0;
        projectile.projectileLifespan = endFadeTime;
        Color tmpColor = spriteRenderer.color;
        GetComponent<SpriteRenderer>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1);
    }

    IEnumerator FadeOut(float duration){
        Color tmpColor = spriteRenderer.color;
        for (float running = 0; running <= duration; running += Time.deltaTime)
        {
            spriteRenderer.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, ((duration - running) / duration) * tmpColor.a);
            yield return null;
        }
        yield return null;
    }
}
