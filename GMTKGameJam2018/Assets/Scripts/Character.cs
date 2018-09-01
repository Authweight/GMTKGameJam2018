using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactive
{
    // Use this for initialization
	void Start ()
    {
        OnStart();
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
	}

    void FixedUpdate()
    {
        OnFixedUpdate();
    }
}
