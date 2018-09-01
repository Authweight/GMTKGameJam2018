using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactive
{
    Animator animator;
    public Smoke smoke;
    private CharacterState state = CharacterState.Idle;

    protected override bool MoveWithConveyor => IsGrounded;

    // Use this for initialization
    void Start ()
    {
        OnStart();
        animator = GetComponent<Animator>();
        onLaunch = OnLaunch;

        onLand = OnLand;
	}
	
    public void OnLaunch()
    {
        if (state == CharacterState.Idle)
        {
            animator.SetTrigger("Launch");
            state = CharacterState.Launching;
        }
    }

    public void OnLand()
    {
        animator.SetTrigger("Land");
        state = CharacterState.Landing;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Instantiate(smoke, new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z - 1), Quaternion.Euler(0, 0, 0), transform);
    }

	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();

        if (state == CharacterState.Launching)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Launch"))
            {
                state = CharacterState.Drifting;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.angularVelocity = -Random.Range(30, 300);
            }
        }

        if (state == CharacterState.Landing)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterIdle"))
            {
                state = CharacterState.Idle;
            }
        }
    }

    private enum CharacterState
    {
        Idle,
        Launching,
        Drifting,
        Landing
    }
}
