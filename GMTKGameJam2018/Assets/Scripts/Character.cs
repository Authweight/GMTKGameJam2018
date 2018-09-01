using Assets.Scripts;
using UnityEngine;

public class Character : Interactive
{
    Animator animator;
    public Smoke smoke;
    private CharacterState state = CharacterState.Idle;

    protected override bool MoveWithConveyor => IsGrounded;

    // Use this for initialization
    void Start ()
    {
        OnStart();
        animator = GetComponent<Animator>();
        onLaunch = OnLaunch;

        onLand = OnLand;
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
        rb.angularVelocity = -Random.Range(30, 300);
    }

    public void OnLand()
    {
        animator.SetTrigger("Land");
        state = CharacterState.Landing;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Instantiate(smoke, new Vector3(transform.position.x, transform.position.y - .05f, transform.position.z - 1), Quaternion.Euler(0, 0, 0));
    }

	// Update is called once per frame
	void Update ()
    {
        OnUpdate();
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

    private enum CharacterState
    {
        Idle,
        Launching,
        Drifting,
        Landing
    }
}
