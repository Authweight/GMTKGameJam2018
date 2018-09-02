using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    public Transform[] rooms;
    float nextRoom;
    public CameraController camera;
    private float offSet;

	// Use this for initialization
	void Start ()
    {
        nextRoom = transform.position.x;
        offSet = transform.position.x - camera.transform.position.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(camera.transform.position.x + offSet, transform.position.y, transform.position.z);
		if (transform.position.x > nextRoom)
        {
            var pick = Random.Range(0, rooms.Length);
            Instantiate(rooms[pick], new Vector3(transform.position.x, 19, 0), Quaternion.identity);
            nextRoom = transform.position.x + 30;
        }
	}
}
