using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class BuffSkill : MonoBehaviour {

	public int playerID;
	public Player nekoyuPlayer;
	[System.NonSerialized]
	private bool initialized;

	public float buffDuration;
	public float buffCoolDown;
	private Animator characterAnimator;
	private float currentBuffDuration;
	private float currentBuffCooldown;
	public static float lifestealAmount;
	public static bool isBuffed;
	public ParticleSystem particles;

	private Buff skillBuff;

	// Use this for initialization
	void Start () {
		currentBuffDuration = 0;
		currentBuffCooldown = 0;
		isBuffed = false;
		characterAnimator = GetComponent<Animator> ();
		lifestealAmount = 0.04f;
		skillBuff = new TestBuff ("Nekoyu's Dominyation", buffDuration, null, false);
	}

	// Update is called once per frame
	void Update () {
		if(!ReInput.isReady) return;
		if(!initialized) nekoyuPlayer = ReInput.players.GetPlayer(playerID);

		if (currentBuffCooldown > 0) {
			currentBuffCooldown -= Time.deltaTime;
		}
		//if (Input.GetKey (KeyCode.V) && currentBuffCooldown <= 0 && !isBuffed) {
		if (nekoyuPlayer.GetButton("L Bumper") && currentBuffCooldown <= 0 && !isBuffed) {
			particles.Play ();
			characterAnimator.Play ("Buff", 0, 0f);
			NekoyuInput.attacking = true;
			Collider2D[] hitList = Physics2D.OverlapCircleAll (transform.position, 30, LayerMask.GetMask ("Player"));
			for (int i = 0; i < hitList.Length; i++) {
				hitList[i].transform.GetComponent<UnitAttributes>().ApplyAttack(0, Vector2.zero, skillBuff);
				Debug.Log ("Buff applied: " + skillBuff.BuffName);

			}
			isBuffed = true;
			StartCoroutine ("waitForSeconds");
		} 

		//CircleCast or CircleOverlap	
		if (isBuffed && currentBuffDuration < buffDuration) {
				currentBuffDuration += Time.deltaTime;
		}

		if (currentBuffDuration >= buffDuration) {
			particles.Stop();
			isBuffed = false;
			currentBuffDuration = 0;

			currentBuffCooldown = buffCoolDown;
		}
	}
		
	IEnumerator waitForSeconds(){
		yield return new WaitForSeconds (1);
		NekoyuInput.attacking = false;
	}

}
