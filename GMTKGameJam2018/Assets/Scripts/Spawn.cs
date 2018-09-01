using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public Bouncer spring;
    private Transform spawnTransform;

    public Queue<Bouncer> bouncers = new Queue<Bouncer>();


	// Use this for initialization
	void Start () {
        spawnTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        var input = Input.GetButtonDown("Fire1");
        if (input)
            SpawnSpring();
	}

    private void SpawnSpring() { 
        var newSpring = Instantiate(spring, spawnTransform.position, spawnTransform.rotation);
        newSpring.spawn = transform;
        bouncers.Enqueue(newSpring);
    }
}
