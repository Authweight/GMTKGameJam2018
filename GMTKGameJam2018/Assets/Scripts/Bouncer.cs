using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private Collider2D myCollider;
    private Rigidbody2D rb;
    private float launchForceX = 1f;
    private float launchForceY = 3;

    private float? deathTime;
    private float deathDelay = 1.2f;


	// Use this for initialization
	void Start () {
        myCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (deathTime.HasValue && TimeKeeper.GetTime() > deathTime)
            Destroy(gameObject);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            var character = collision.gameObject.GetComponent<Character>();
            character.Launch();
            Launch();
        }
    }

    private void Launch()
    {
        deathTime = TimeKeeper.GetTime() + deathDelay;
        rb.velocity = new Vector2(launchForceX, launchForceY);
    }
}
