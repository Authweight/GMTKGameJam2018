using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteSpring : Interactive
{
    private SpringManager springManager;

    public Transform spawn;

    private float slowDrop = .1f;

    private float deployHeight = 5.0f;

    protected override bool MoveWithConveyor => false;

    // Use this for initialization
    void Start()
    {
        springManager = new SpringManager();
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
        if (springManager.Destroy())
            Destroy(gameObject);
        springManager.HandleDrop(transform, spawn, IsGrounded);
        if (springManager.IsDropping())
        {
            var shouldDeploy = CheckForDeploy();
            if (shouldDeploy)
            {
                springManager.Deploy();
            }
        }
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
        rb.velocity = springManager.GetVelocity(rb.velocity);
        if (springManager.IsDeploying())
        {
            rb.velocity = new Vector2(0, -slowDrop);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var interactive = collision.gameObject.GetComponent<Interactive>();
        if (interactive != null)
        {
            springManager.Extend(interactive, rb, transform);
        }
    }

    public bool CheckForDeploy()
    {
        return RaycastCheck(0f) || RaycastCheck(-.5f) || RaycastCheck(.5f);
    }

    public bool RaycastCheck(float offset)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + offset, transform.position.y), Vector2.down, deployHeight, LayerMask.GetMask("Deployer", "Conveyor"));
    }

    private enum DeployState
    {
        Dropping,
        Deploying,
        Finished
    }
}
