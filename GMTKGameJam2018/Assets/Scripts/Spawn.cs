﻿using Assets.Scripts;
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

    public Rigidbody2D[] potatoes;

    private float timeBetweenPotatoes = 1.5f;
    private int potatoesLeft;
    private float nextPotato;
    
	// Use this for initialization
	void Start ()
    {
        spawnTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        PotatoSwitchEvents.RegisterListener(() => SpawnPotatoes());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!gameOver)
        {
            var spring = GetSpringInput();
            
            if (spring)
                SpawnSpring();

            var parachute = GetParachuteInput(); 
            if (parachute)
                SpawnParachute();
        }

        SpawnPotatoIfNeeded();
	}

    private bool GetParachuteInput()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        return Input.GetButtonDown("Fire2");
#endif
#if UNITY_ANDROID
        return MobileParachute();
#endif
    }

    private bool GetSpringInput()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        return Input.GetButtonDown("Fire1");
#endif
#if UNITY_ANDROID
        return MobileSpring();
#endif
    }

    private bool MobileParachute()
    {
        return TouchInPosition(x => x > (Screen.width / 2));
    }

    private bool MobileSpring()
    {
        return TouchInPosition(x => x <= (Screen.width / 2));
    }

    private bool TouchInPosition(Func<float, bool> comparer)
    {
        if (Input.touchCount == 0)
            return false;

        var touch = Input.touches[0];
        return touch.phase == TouchPhase.Ended && comparer(touch.position.x);
    }

    private void SpawnParachute()
    {
        animator.SetTrigger("Spawn");
        SoundEvents.Play("ItemDrop");
        var newParachute = Instantiate(parachute, GetSpawnPosition(), spawnTransform.rotation);
        newParachute.spawn = transform;
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
    }

    public void SpawnPotatoes()
    {
        potatoesLeft = 3;
        nextPotato = TimeKeeper.GetTime();
    }

    private void SpawnPotatoIfNeeded()
    {
        if (potatoesLeft > 0 && TimeKeeper.GetTime() > nextPotato)
        {
            animator.SetTrigger("Spawn");
            SoundEvents.Play("ItemDrop");
            var newPotato = Instantiate(potatoes[3-potatoesLeft], GetSpawnPosition(), spawnTransform.rotation);
            newPotato.velocity = new Vector2(ConveyorSpeed.GetSpeed(), 0);
            newPotato.angularVelocity = UnityEngine.Random.Range(40, 180);
            potatoesLeft--;
            nextPotato = TimeKeeper.GetTime() + timeBetweenPotatoes;                
        }
    }

    private void SpawnSpring()
    {
        animator.SetTrigger("Spawn");
        SoundEvents.Play("ItemDrop");
        var newSpring = Instantiate(spring, GetSpawnPosition(), spawnTransform.rotation);
        newSpring.spawn = transform;
    }

    internal void GameOver()
    {
        gameOver = true;
    }
}
