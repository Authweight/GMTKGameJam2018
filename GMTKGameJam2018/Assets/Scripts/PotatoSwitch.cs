using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoSwitch : MonoBehaviour
{
    private bool lever = false;
    private Animator animator;
    private float resetTime;

    private float resetDelay = 3.0f;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (lever && TimeKeeper.GetTime() > resetTime)
        {
            lever = false;
            animator.SetBool("Switch", false);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!lever)
        {
            lever = true;
            animator.SetBool("Switch", true);
            resetTime = TimeKeeper.GetTime() + resetDelay;
            PotatoSwitchEvents.SwitchTriggered();
        }
    }
}
