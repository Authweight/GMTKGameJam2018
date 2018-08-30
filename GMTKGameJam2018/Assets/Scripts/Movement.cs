using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Rigidbody2D rb;
    private const float speed = 10;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        var input = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(input * speed, 0);
	}
}
