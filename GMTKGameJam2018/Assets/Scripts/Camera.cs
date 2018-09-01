﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform character;
    public float offset;

	// Use this for initialization
	void Start ()
    {
        offset = transform.position.x - character.position.x;
	}

    public void Update()
    {
        transform.position = new Vector3(Mathf.Max(character.position.x + offset), transform.position.y, transform.position.z);
    }
}
