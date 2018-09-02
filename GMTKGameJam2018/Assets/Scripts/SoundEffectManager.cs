using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SoundEvents.AddSources(GetComponentsInChildren<AudioSource>());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
