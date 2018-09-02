using Assets.Scripts;
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
            var spring = Input.GetButtonDown("Fire1");
            if (spring)
                SpawnSpring();
            var parachute = Input.GetButtonDown("Fire2");
            if (parachute)
                SpawnParachute();
        }

        SpawnPotatoIfNeeded();
	}

    private void SpawnParachute()
    {
        animator.SetTrigger("Spawn");
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
        var newSpring = Instantiate(spring, GetSpawnPosition(), spawnTransform.rotation);
        newSpring.spawn = transform;
    }

    internal void GameOver()
    {
        gameOver = true;
    }
}
