using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private Scene scene;
    private int score = 0;
    public Text scoreText;
    public Image deathOverlay;
    public Text finalScore;
    public Spawn spawn;
    private bool gameOver = false;

    private readonly byte deathOverlayOpacity = 130;

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
        if (!gameOver)
        {
            score += amount;
            scoreText.text = $"Score: {score}";
            finalScore.text = $"Final Score \n {score}";
        }
    }

    public void Death()
    {
        deathOverlay.color = new Color32(0, 0, 0, deathOverlayOpacity);
        finalScore.color = new Color(finalScore.color.r, finalScore.color.g, finalScore.color.b, 255);
        gameOver = true;
        spawn.GameOver();
    }
}
