using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [HideInInspector]
    public StateMachine<PlayerMovement> stateMachine;

    [HideInInspector]
    public JumpingState jumpingState;
    [HideInInspector]
    public OnGroundState onGroundState;
    [HideInInspector]
    public FallingState fallingState;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PlayerInput input;
    [HideInInspector]
    public Rigidbody2D rigidBody2d;
    [HideInInspector]
    public PlayerStats playerStats;
    [HideInInspector]
    public PlayerAudio playerAudio;

    private const float skinWidth = .015f;
    [SerializeField]
    private LayerMask groundLayer;          
    private const float groundDistance = 0.1f;        //Distance player is considered to be on the ground
    [SerializeField]
    private LayerMask bouncyBallLayer;


    private int animatorVelocityY_ID;
    private int animatorVelocityX_ID;
    private int animatorTimeStill_ID;

    private float timeStill = 0;



    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        
    }

    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<PlayerAudio>();

        jumpingState = new JumpingState(this);
        onGroundState = new OnGroundState(this);
        fallingState = new FallingState(this);

        animatorVelocityX_ID = Animator.StringToHash("VelocityX");
        animatorVelocityY_ID = Animator.StringToHash("VelocityY");
        animatorTimeStill_ID = Animator.StringToHash("TimeStill");

        stateMachine = new StateMachine<PlayerMovement>(this);

        if(LegOnGround())
        {
            stateMachine.ChangeState(onGroundState);
        }
        else
        {
            stateMachine.ChangeState(fallingState);
        }
        
    }

    void Update()
    {
        animator.SetFloat(animatorVelocityY_ID, rigidBody2d.velocity.y);
        animator.SetFloat(animatorVelocityX_ID, Mathf.Abs(rigidBody2d.velocity.x));
        stateMachine.Update();
        if(rigidBody2d.velocity.x == 0)
        {
            timeStill += Time.deltaTime;
        }
        else
        {
            timeStill = 0;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();

        float xVelocity = playerStats.Speed * input.horizontal;
        rigidBody2d.velocity = new Vector2(xVelocity, rigidBody2d.velocity.y);
    }

    public bool LegOnGround()
    {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        bounds.Expand(skinWidth * -2);

        Vector2 bottomLeftCorner = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bottomRightCorner = new Vector2(bounds.max.x, bounds.min.y);

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Physics2D.Raycast(bottomLeftCorner, Vector2.down, groundDistance, groundLayer);
        if(leftCheck)
        {
            Debug.DrawRay(bottomLeftCorner, Vector2.down, Color.green);
        }
        else
        {
            Debug.DrawRay(bottomLeftCorner, Vector2.down, Color.red);
        }
        
        RaycastHit2D rightCheck = Physics2D.Raycast(bottomRightCorner, Vector2.down, groundDistance, groundLayer);
        if (rightCheck)
        {
            Debug.DrawRay(bottomRightCorner, Vector2.down, Color.green);
        }
        else
        {
            Debug.DrawRay(bottomRightCorner, Vector2.down, Color.red);
        }

        return (leftCheck || rightCheck);
    }

    public bool LegOnBouncyBall()
    {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        bounds.Expand(skinWidth * -2);

        Vector2 bottomLeftCorner = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bottomRightCorner = new Vector2(bounds.max.x, bounds.min.y);

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Physics2D.Raycast(bottomLeftCorner, Vector2.down, groundDistance, bouncyBallLayer);
        RaycastHit2D rightCheck = Physics2D.Raycast(bottomRightCorner, Vector2.down, groundDistance, bouncyBallLayer);

        return (leftCheck || rightCheck);
    }
}
