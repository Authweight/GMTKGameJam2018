using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : Interactive
{
    private DeployState deployState = DeployState.Dropping;

    private float extensionForceX = 1f;
    private float extensionForceY = 3;

    private float? deathTime;
    private float deathDelay = 1.2f;

    private float maxHardDrop = 30f;
    private float slowDrop = .1f;

    private float deployHeight = 3.8f;

    protected override bool MoveWithConveyor => 
        deployState == DeployState.Dropping  
        ? true 
        : deployState == DeployState.Deploying 
            ? false 
            : isGrounded;


	// Use this for initialization
	void Start ()
    {
        OnStart();
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
        if (deathTime.HasValue && TimeKeeper.GetTime() > deathTime)
            Destroy(gameObject);

        if (deployState == DeployState.Dropping)
        {
            var shouldDeploy = CheckForDeploy();
            if (shouldDeploy)
            {
                deployState = DeployState.Deploying;
            }
        }
        if (deployState == DeployState.Deploying)
        {
            if (isGrounded)
            {
                deployState = DeployState.Finished;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
	}

    private void FixedUpdate()
    {
        OnFixedUpdate();
        if (deployState == DeployState.Dropping)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxHardDrop));
        }
        if (deployState == DeployState.Deploying)
        {
            rb.velocity = new Vector2(0, -slowDrop);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var interactive = collision.gameObject.GetComponent<Interactive>();
        if (interactive != null)
        {
            interactive.SpringLaunch();
            Extend();
        }
    }

    public bool CheckForDeploy()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, deployHeight, LayerMask.GetMask("Conveyor"));
    }

    private void Extend()
    {
        deployState = DeployState.Finished;
        deathTime = TimeKeeper.GetTime() + deathDelay;
        rb.velocity = new Vector2(extensionForceX, extensionForceY);
    }

    private enum DeployState
    {
        Dropping,
        Deploying,
        Finished
    }
}
