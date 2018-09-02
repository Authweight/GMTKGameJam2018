using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static bool loaded;
	// Use this for initialization
	void Start ()
    {
        if (loaded)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        loaded = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
