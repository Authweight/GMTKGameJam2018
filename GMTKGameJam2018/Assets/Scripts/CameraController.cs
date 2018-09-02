using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform character;
    private float offset;

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
