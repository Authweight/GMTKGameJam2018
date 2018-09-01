using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform character;
    public float maxOffset;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        maxOffset = transform.position.x - character.position.x;
	}

    public void Update()
    {
        transform.position = new Vector3(Mathf.Max(transform.position.x, character.position.x + maxOffset), transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(ConveyorSpeed.GetSpeed() * 0.9f, transform.position.y);
	}
}
