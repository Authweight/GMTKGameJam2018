using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public Spring spring;
    public ParachuteSpring parachute;
    private Transform spawnTransform;


	// Use this for initialization
	void Start () {
        spawnTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        var spring = Input.GetButtonDown("Fire1");
        if (spring)
            SpawnSpring();
        var parachute = Input.GetButtonDown("Fire2");
        if (parachute)
            SpawnParachute();

	}

    private void SpawnParachute()
    {
        var newParachute = Instantiate(parachute, spawnTransform.position, spawnTransform.rotation);
        newParachute.spawn = transform;
    }

    private void SpawnSpring()
    { 
        var newSpring = Instantiate(spring, spawnTransform.position, spawnTransform.rotation);
        newSpring.spawn = transform;
    }
}
