﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImbueProjectileController : MonoBehaviour {

    FadeoutController fadeoutController;

    private void Awake()
    {
        fadeoutController = GetComponent<FadeoutController>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<ImbueableProjectileController>()){
            collision.transform.GetComponent<ImbueableProjectileController>().Imbue();
            fadeoutController.Refresh();
        }
    }
}
