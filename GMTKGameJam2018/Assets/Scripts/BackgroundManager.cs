using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform farBackground;
    public Transform midBackground;
    public Transform[] nearBackgrounds;

    private List<ParallaxObject> activeFarBackgrounds;
    private List<ParallaxObject> activeMidBackgrounds;
    private List<ParallaxObject> activeNearBackgrounds;

    private const float farParallax = .9f;
    private const float midParallax = .5f;
    private float nearParallax = .9f;

    private float tileWidth = 30;    

	// Use this for initialization
	void Start ()
    {
        activeFarBackgrounds = Create(farBackground, farParallax, 50);
        activeMidBackgrounds = Create(midBackground, midParallax, 40);
        activeNearBackgrounds = new List<ParallaxObject>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        UpdateAll();
        JumpBackgrounds(cameraTransform, activeFarBackgrounds);
        JumpBackgrounds(cameraTransform, activeMidBackgrounds);
	}

    private void JumpBackgrounds(Transform cameraTransform, List<ParallaxObject> backgrounds)
    {
        ParallaxObject left = backgrounds[0];
        ParallaxObject right = backgrounds[0];
        foreach(var background in backgrounds)
        {
            if (background.Position < left.Position)
                left = background;
            if (background.Position > right.Position)
                right = background;
        }

        if (right.Position - cameraTransform.position.x < tileWidth / 2)
            left.Jump(tileWidth * 5);
        else if (cameraTransform.position.x - left.Position < tileWidth / 2)
            right.Jump(-tileWidth * 5);
    }

    private void UpdateAll()
    {
        activeFarBackgrounds.ForEach(x => x.UpdatePosition());
        activeMidBackgrounds.ForEach(x => x.UpdatePosition());
        activeNearBackgrounds.ForEach(x => x.UpdatePosition());
    }

    private List<ParallaxObject> Create(Transform background, float parallax, float plane)
    {
        var middle = Instantiate(background, new Vector3(cameraTransform.position.x, cameraTransform.position.y, plane), Quaternion.identity);
        var left = Instantiate(background, new Vector3(cameraTransform.position.x - tileWidth, cameraTransform.position.y, plane), Quaternion.identity);
        var farLeft = Instantiate(background, new Vector3(cameraTransform.position.x - tileWidth * 2, cameraTransform.position.y, plane), Quaternion.identity);
        var right = Instantiate(background, new Vector3(cameraTransform.position.x + tileWidth, cameraTransform.position.y, plane), Quaternion.identity);
        var farRight = Instantiate(background, new Vector3(cameraTransform.position.x + tileWidth * 2, cameraTransform.position.y, plane), Quaternion.identity);

        return new List<ParallaxObject>
        {
            new ParallaxObject(middle.transform, cameraTransform, parallax, cameraTransform.position.x),
            new ParallaxObject(left.transform, cameraTransform, parallax, cameraTransform.position.x),
            new ParallaxObject(farLeft.transform, cameraTransform, parallax, cameraTransform.position.x),
            new ParallaxObject(right.transform, cameraTransform, parallax, cameraTransform.position.x),
            new ParallaxObject(farRight.transform, cameraTransform, parallax, cameraTransform.position.x),
        };
    }

    private class ParallaxObject
    {
        private Transform cameraTransform;
        private Transform transform;
        private float parallax;
        private float alignmentPoint;
        private float start;

        public void UpdatePosition()
        {
            transform.position = new Vector3(start + (cameraTransform.position.x - alignmentPoint) * parallax, transform.position.y, transform.position.z);
        }

        public ParallaxObject(Transform transform, Transform camera, float parallax, float alignmentPoint)
        {
            this.transform = transform;
            this.cameraTransform = camera;
            this.parallax = parallax;
            this.alignmentPoint = alignmentPoint;
            this.start = transform.position.x;
        }

        public float Position => transform.position.x;        

        public void Jump(float amount)
        {
            Debug.Log("Jumping");
            start += amount;
            UpdatePosition();
        }
    }
}
