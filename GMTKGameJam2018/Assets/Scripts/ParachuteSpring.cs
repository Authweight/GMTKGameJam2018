using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteSpring : Interactive
{
    private SpringManager springManager;
    public Parachute parachutePrefab;

    public Transform spawn;

    private float deployHeight = 5.0f;
    private const float VeryLarge = 1000000000f;
    private HingeJoint2D joint;
    private Parachute myParachute;

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
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
        rb.velocity = springManager.GetVelocity(rb.velocity);

        if (springManager.IsDropping())
        {
            var shouldDeploy = CheckForDeploy();
            if (shouldDeploy)
            {
                rb.velocity = Vector2.zero;
                myParachute = Instantiate(parachutePrefab, transform.position, Quaternion.identity);
                myParachute.transform.position = transform.position + Vector3.up * 1f;
                joint = gameObject.AddComponent<HingeJoint2D>();
                joint.connectedBody = myParachute.GetComponent<Rigidbody2D>();
                joint.anchor = new Vector2(0, .59f);
                joint.connectedAnchor = new Vector2(0.05f, -0.6f);
                joint.limits = new JointAngleLimits2D { max = 359, min = 0 };
                joint.breakForce = VeryLarge;
                joint.breakTorque = VeryLarge;
                joint.autoConfigureConnectedAnchor = true;
                springManager.Deploy(rb);
            }
        }

        if (springManager.IsFinished() && joint != null)
        {
            Destroy(joint);
            myParachute.CutLoose();
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
