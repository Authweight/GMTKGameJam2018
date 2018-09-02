using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Transform[] groundPieces;
    public Transform player;

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

    private void FixedUpdate()
    {
        HandleGroundTransformation();
    }

    private void HandleGroundTransformation()
    {
        var playerX = player.position.x;
        Transform greatestGroundX = groundPieces[0];
        Transform leastGroundX = groundPieces[0];

        foreach(var piece in groundPieces)
        {
            if (piece.position.x > greatestGroundX.position.x)
                greatestGroundX = piece;
            if (piece.position.x < leastGroundX.position.x)
                leastGroundX = piece;
        }

        if (greatestGroundX.position.x - playerX < 30)
        {
            leastGroundX.position = new Vector3(greatestGroundX.position.x + 99, leastGroundX.position.y);
        }
        if (playerX - leastGroundX.position.x < 30)
        {
            greatestGroundX.position = new Vector3(leastGroundX.position.x - 99, greatestGroundX.position.y);
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
