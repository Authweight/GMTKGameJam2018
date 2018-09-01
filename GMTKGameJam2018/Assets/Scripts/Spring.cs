using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Interactive
{
    private SpringManager springManager;
    public Transform spawn;
    protected override bool MoveWithConveyor => false;


    // Use this for initialization
    void Start ()
    {
        springManager = new SpringManager();
        OnStart();
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
        if (springManager.Destroy())
        {
            Destroy(gameObject);
        };

        springManager.HandleDrop(transform, spawn, IsGrounded);
	}

    private void FixedUpdate()
    {
        OnFixedUpdate();

        rb.velocity = springManager.GetVelocity(rb.velocity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var interactive = collision.gameObject.GetComponent<Interactive>();
        if (interactive != null)
        {
            springManager.Extend(interactive, rb, transform);
        }
    }
}
