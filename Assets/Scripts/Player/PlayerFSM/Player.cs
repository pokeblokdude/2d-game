using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {
    
    #region State Machine Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public Idle IdleState { get; private set; }
    public Leaning LeaningState { get; private set; }
    public Moving MovingState { get; private set; }
    public Running RunningState { get; private set; }
    public Crouching CrouchingState { get; private set; }
    public CrouchingMoving CrouchingMovingState { get; private set; }

    public HardLanding HardLandingState { get; private set; }

    public Falling FallingState { get; private set; }
    public FallingFromJump FallingFromJumpState { get; private set; }
    public Jumping JumpingState { get; private set; }
    public AirDiving AirDivingState { get; private set; }
    public AirDivingFreeze AirDivingFreezeState { get; private set; }

    public Rolling RollingState { get; private set; }

    public Bonked BonkedState { get; private set; }

    public WallSliding WallSlidingState { get; private set; }
    public WallSlidingNoLedgeGrab WallSlidingNoLedgeGrabState { get; private set; }
    public WallJumping WallJumpingState { get; private set; }

    public ClippingLedge ClippingLedgeState { get; private set; }
    public GrabbingLedge GrabbingLedgeState { get; private set; }
    public ClimbingLedge ClimbingLedgeState { get; private set; }
   
    #endregion

    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public BoxCollider2D col { get; private set; }

    [SerializeField] PlayerData playerData;
    [SerializeField] public LedgeGrabPoint ledgeGrabPoint;
    [SerializeField] Text text;

    public Controller2D controller { get; private set; }
    public PlayerInput input { get; private set; }

    public Vector3 wishVelocity { get; private set; }
    public Vector3 actualVelocity { get; private set; }

    public bool CALCULATE_COLLISION = true;

    void Awake() {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<Controller2D>();

        #region SM Init
        StateMachine = new PlayerStateMachine();

        IdleState = new Idle(this, StateMachine, playerData, "idle");
        LeaningState = new Leaning(this, StateMachine, playerData, "leaning");
        MovingState = new Moving(this, StateMachine, playerData, "moving");
        RunningState = new Running(this, StateMachine, playerData, "running");
        CrouchingState = new Crouching(this, StateMachine, playerData, "crouching");
        CrouchingMovingState = new CrouchingMoving(this, StateMachine, playerData, "crouchingMoving");

        HardLandingState = new HardLanding(this, StateMachine, playerData, "hardLanding");

        JumpingState = new Jumping(this, StateMachine, playerData, "jumping");
        FallingState = new Falling(this, StateMachine, playerData, "falling");
        FallingFromJumpState = new FallingFromJump(this, StateMachine, playerData, "fallingFromJump");
        AirDivingState = new AirDiving(this, StateMachine, playerData, "airDiving");
        AirDivingFreezeState = new AirDivingFreeze(this, StateMachine, playerData, "airDivingFreeze");

        RollingState = new Rolling(this, StateMachine, playerData, "rolling");

        BonkedState = new Bonked(this, StateMachine, playerData, "bonked");

        WallSlidingState = new WallSliding(this, StateMachine, playerData, "wallSliding");
        WallSlidingNoLedgeGrabState = new WallSlidingNoLedgeGrab(this, StateMachine, playerData, "wallSliding");
        WallJumpingState = new WallJumping(this, StateMachine, playerData, "wallJumping");

        ClippingLedgeState = new ClippingLedge(this, StateMachine, playerData, "clippingLedge");
        GrabbingLedgeState = new GrabbingLedge(this, StateMachine, playerData, "grabbingLedge");
        ClimbingLedgeState = new ClimbingLedge(this, StateMachine, playerData, "climbingLedge");
        #endregion
    }

    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        StateMachine.Initialize(IdleState);
    }

    void Update() {
        StateMachine.CurrentState.LogicUpdate();
        if(CALCULATE_COLLISION) {
            actualVelocity = controller.Move(wishVelocity * Time.deltaTime);
        }
        Debug.DrawRay(transform.position, actualVelocity, Color.green, Time.deltaTime);
        Debug.DrawRay(transform.position, wishVelocity, Color.blue, Time.deltaTime);
        setDebugText();
        anim.SetFloat("vSpeed", actualVelocity.y);
        
        controller.CalculateRaySpacing();
    }

    void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void setVelX(float f) {
        wishVelocity = new Vector3(f, wishVelocity.y, wishVelocity.z);
    }

    public void setVelY(float f) {
        wishVelocity = new Vector3(wishVelocity.x, f, wishVelocity.z);
    }

    public void setVelZero() {
        wishVelocity = Vector2.zero;
        actualVelocity = Vector2.zero;
    }

    public void setVelActual(Vector2 vel) {
        actualVelocity = vel;
    }

    public void setVelActual(float x, float y) {
        actualVelocity = new Vector3(x, y, 0);
    }

    public float Accelerate(float wishDir, float acceleration, float maxSpeed, float friction) {
        float speed = actualVelocity.x;
        if(wishDir != 0) {
            // 330 was about the framerate I was playing at before I fixed the deltaTime issue
            if(Mathf.Abs(speed) < maxSpeed) {
                speed += wishDir * 330 * Time.deltaTime * acceleration / friction;
            }
            if(Mathf.Abs(speed) > maxSpeed) {
                speed -= Mathf.Sign(speed) * friction * acceleration * Time.deltaTime;
            }
        }
        else {
            float decelSpeed = speed - Mathf.Sign(speed) * friction * Time.deltaTime;
            speed = (Mathf.Sign(decelSpeed) != Mathf.Sign(speed)) ? 0 : decelSpeed;
        }
        return speed;
    }

    public float AirAccelerate(float wishDir, float airAcceleration, float maxAirSpeed, float airFriction) {
        float speed = actualVelocity.x;
        if(input.moveDir != 0) {
            if(Mathf.Sign(wishDir) != Mathf.Sign(speed) || Mathf.Abs(speed) < maxAirSpeed) {
                speed += wishDir * 330 * Time.deltaTime * airAcceleration / airFriction;
            }
        }
        return speed;
    }

    void setDebugText() {
        text.text =     $"FPS: {(1/Time.deltaTime).ToString("F0")}\n" +
                        $"deltaTime: {Time.deltaTime}\n" +
                        $"Daylight Ticks: {Mathf.FloorToInt(Mathf.Repeat(Time.time, 1200))}\n\n" +
                        $"Current State: {StateMachine.CurrentState.Name()}\n" +
                        $"Speed: {actualVelocity.magnitude.ToString("f2")}\n" +
                        $"HSpeed: {actualVelocity.x.ToString("F2")}\n" +
                        $"VSpeed: {actualVelocity.y.ToString("F2")}\n" +
                        $"WishVel: {wishVelocity.ToString("F4")}\n" +
                        $"Position: {transform.position.ToString("F4")}\n" +
                        $"MoveDir: {input.moveDir}\n" +
                        $"Grounded: {controller.isGrounded()}\n" +
                        $"Calculating Gravity: {StateMachine.CurrentState.calculatingGravity()}\n" +
                        $"Can Uncrouch: {controller.canUncrouch()}\n" +
                        $"Bumping Head: {controller.isBumpingHead()}\n" +
                        $"Touching Wall: {controller.isTouchingWall()}\n"
                        
        ;
    }

}
