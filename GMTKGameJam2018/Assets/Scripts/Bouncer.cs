using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : Interactive
{
    private DeployState deployState = DeployState.Dropping;
    public Transform spawn;

    private float extensionForceX = 1f;
    private float extensionForceY = 3;

    private float? deathTime;
    private float deathDelay = 1.2f;

    private float maxHardDrop = 30f;
    private float slowDrop = .1f;

    private float deployHeight = 3.8f;

    private float defaultGravity;

    protected override bool MoveWithConveyor => deployState == DeployState.Finished ? true : false;


    // Use this for initialization
    void Start ()
    {
        OnStart();
        defaultGravity = rb.gravityScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
        if (deathTime.HasValue && TimeKeeper.GetTime() > deathTime)
            Destroy(gameObject);

        if (deployState == DeployState.Dropping)
        {
            transform.position = new Vector3(spawn.position.x, transform.position.y, transform.position.z);
            var shouldDeploy = CheckForDeploy();
            if (shouldDeploy)
            {
                deployState = DeployState.Deploying;
                //rb.gravityScale = 0;
            }
        }
        if (deployState == DeployState.Deploying)
        {
            if (isGrounded)
            {
                deployState = DeployState.Finished;
                rb.gravityScale = defaultGravity;
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
        return RaycastCheck(0f) || RaycastCheck(-.5f) || RaycastCheck(.5f);
    }

    public bool RaycastCheck(float offset)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + offset, transform.position.y), Vector2.down, deployHeight, LayerMask.GetMask("Conveyor"));
    }

    private void Extend()
    {
        deployState = DeployState.Finished;
        rb.gravityScale = defaultGravity;
        deathTime = TimeKeeper.GetTime() + deathDelay;
        rb.velocity = new Vector2(extensionForceX, extensionForceY);
    }

    private enum DeployState
    {
        Dropping,
        Deploying,
        Finished
    }

    public void Drop()
    {
        deployState = DeployState.Finished;
    }

    public bool IsDroppable()
    {
        return gameObject.activeInHierarchy && !(deployState == DeployState.Finished);
    }
}
