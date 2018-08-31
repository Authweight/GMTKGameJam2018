using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isLaunching = false;
    private float launchX = 2f;
    private float launchY = 25f;
    private float lastLaunch = 0;
    private float launchDelay = .3f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        if (TimeKeeper.GetTime() - lastLaunch > launchDelay)
            isLaunching = false;

        if (isGrounded && !isLaunching)
            rb.velocity = new Vector2(CharacterSpeed.GetSpeed(), rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void Launch()
    {
        if (!isLaunching)
        {
            lastLaunch = TimeKeeper.GetTime();
            isLaunching = true;
            rb.velocity = new Vector2(launchX + rb.velocity.x, launchY);
        }
    }
}
