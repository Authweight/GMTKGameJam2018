using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float jumpDelay = 3.5f;
    private float nextJump;
    private Rigidbody2D rb;
    private float jumpAmount = 13.0f;
    private Animator animator;
    private float jumpTime;
    private bool isGrounded = true;

	// Use this for initialization
	void Start ()
    {
        nextJump = TimeKeeper.GetTime() + jumpDelay;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (TimeKeeper.GetTime() > nextJump)
            Jump();
        var getGrounded = CalculateGrounded();
        if (!isGrounded)
        {
            if (getGrounded)
                animator.SetTrigger("Land");
        }

        isGrounded = getGrounded;
	}

    public bool CalculateGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, .5f, LayerMask.GetMask("Conveyor", "TopRail"));
    }

    private void Jump()
    {
        rb.velocity = new Vector2(0, jumpAmount);
        nextJump = TimeKeeper.GetTime() + jumpDelay;
        animator.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spring"))
            Destroy(gameObject);
    }
}
