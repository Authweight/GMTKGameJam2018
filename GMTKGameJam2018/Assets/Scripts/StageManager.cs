using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private Scene scene;
    private int score;
    public Text scoreText;

	// Use this for initialization
	void Start ()
    {
        scene = SceneManager.GetActiveScene(); 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            SceneManager.LoadScene(scene.name);
        }
	}

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
    }
}
