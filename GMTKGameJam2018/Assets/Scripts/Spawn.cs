﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public Rigidbody2D spring;
    private Transform spawnTransform;
    private float launchSpeed = 3;

	// Use this for initialization
	void Start () {
        spawnTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        var input = Input.GetButtonDown("Fire1");
        if (input)
            SpawnSpring();
	}

    private void SpawnSpring() { 
        var newSpring = Instantiate(spring, spawnTransform.position, spawnTransform.rotation);
        newSpring.velocity = new Vector2(CharacterSpeed.GetSpeed() * .75f, -launchSpeed);
    }
}