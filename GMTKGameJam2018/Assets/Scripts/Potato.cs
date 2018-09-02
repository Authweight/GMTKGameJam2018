using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
    private float lifetime = 9.0f;
    private float timeToDie;

	// Use this for initialization
	void Start ()
    {
        timeToDie = TimeKeeper.GetTime() + lifetime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (TimeKeeper.GetTime() > timeToDie)
            Destroy(gameObject);

	}
}
