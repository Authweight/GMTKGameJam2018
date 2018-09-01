using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public Spring spring;
    public ParachuteSpring parachute;
    private Transform spawnTransform;
    private Animator animator;
    private bool gameOver = false;


	// Use this for initialization
	void Start ()
    {
        spawnTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!gameOver)
        {
            var spring = Input.GetButtonDown("Fire1");
            if (spring)
                SpawnSpring();
            var parachute = Input.GetButtonDown("Fire2");
            if (parachute)
                SpawnParachute();
        }
	}

    private void SpawnParachute()
    {
        animator.SetTrigger("Spawn");
        var newParachute = Instantiate(parachute, spawnTransform.position, spawnTransform.rotation);
        newParachute.spawn = transform;
    }

    private void SpawnSpring()
    {
        animator.SetTrigger("Spawn");
        var newSpring = Instantiate(spring, spawnTransform.position, spawnTransform.rotation);
        newSpring.spawn = transform;
    }

    internal void GameOver()
    {
        gameOver = true;
    }
}
