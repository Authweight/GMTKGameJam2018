using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Character : Interactive
{
    Animator animator;
    public Smoke smoke;
    public StageManager stageManager;
    private CharacterState state = CharacterState.Idle;
    private float scoreMultiplier = 1;
    public Text multiplier;
    private float multiplierEnd;

    private Vector2[] bumperLaunchVectors;

    protected override bool MoveWithConveyor => IsGrounded;

    // Use this for initialization
    void Start ()
    {
        OnStart();
        animator = GetComponent<Animator>();
        onLaunch = OnLaunch;

        onLand = OnLand;
        bumperLaunchVectors = new Vector2[]
        {
            new Vector2(1, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized,
            new Vector2(0, -1).normalized
        };
	}
	
    public void OnLaunch()
    {
        animator.SetTrigger("Launch");
        if (state == CharacterState.Idle)
        {
            state = CharacterState.Launching;
        }
        else
        {
            SetAdrift();
        }
    }

    private void SetAdrift()
    {
        state = CharacterState.Drifting;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = -UnityEngine.Random.Range(30, 300);
    }

    public void OnLand()
    {
        animator.SetTrigger("Land");
        state = CharacterState.Landing;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Instantiate(smoke, new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z - 1), Quaternion.Euler(0, 0, 0));
        multiplierEnd = TimeKeeper.GetTime() + 1.5f;
    }

	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
        if (state == CharacterState.Idle && TimeKeeper.GetTime() > multiplierEnd)
        {
            scoreMultiplier = 1;
            multiplier.text = "";
        }
    }

    void FixedUpdate()
    {
        OnFixedUpdate();

        if (state == CharacterState.Launching)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Launch"))
            {
                SetAdrift();
            }
        }

        if (state == CharacterState.Landing)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterIdle"))
            {
                state = CharacterState.Idle;
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Dummy"))
            {
                animator.SetTrigger("Land"); //Sometimes it needs a little extra nudge to get into the right animation
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var collectible = collider.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            stageManager.AddScore(Mathf.RoundToInt(collectible.Collect() * scoreMultiplier));
            scoreMultiplier += .2f;
            multiplier.text = $"x{scoreMultiplier:f1}";
        }

        if (collider.gameObject.CompareTag("Hazard"))
        {
            stageManager.Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            var vector = (transform.position - collision.transform.position).normalized;
            var launchVector = NearestVector(vector) * 20.0f;
            SpringLaunch(launchVector.x, launchVector.y);
        }

        if (collision.gameObject.CompareTag("Hazard"))
        {
            stageManager.Death();
        }
    }

    private Vector2 NearestVector(Vector3 vector)
    {
        var actual = new Vector2(vector.x, vector.y);
        return bumperLaunchVectors.OrderBy(x => Vector2.Angle(x, actual)).First();
    }

    private enum CharacterState
    {
        Idle,
        Launching,
        Drifting,
        Landing
    }
}
