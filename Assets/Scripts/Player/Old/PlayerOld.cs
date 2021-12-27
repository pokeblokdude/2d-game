 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Controller2D))]
public class PlayerOld : MonoBehaviour {

    public Text debugText;
    [Header("Movement")]
    [SerializeField] float maxSpeed = 5f;
    [SerializeField][Range(0, 1)] float crouchSpeedMult = 0.5f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float airAcceleration = 2f;
    [SerializeField] float friction = 5f;
    [SerializeField] float crouchFrictionMult = 2f;
    [SerializeField] float airFriction = 30f;
    [SerializeField] float gravity = 5;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10;
    [SerializeField] float coyoteTime = 0.1f;

    [Header("Wall-slide Settings")]
    [SerializeField] float wallSlideFriction = 4f; 
    [SerializeField] float maxWallSlideSpeed = 3f;
    
    [Header("Air Dive Settings")]
    [SerializeField] float diveForce = 5f;

    [Header("Misc")]
    [SerializeField] float bonkDuration = 1;

    Vector3 wishVelocity;
    Vector3 actualVelocity;
    float moveDir;

    bool jump;
    bool jumping = false;
    bool canJumpAgain = true;
    float jumpDir = 0;
    bool coyote = false;
    float coyoteTimestamp;

    bool grounded;
    bool wasGrounded;
    bool moving = false;
    bool falling;

    int touchingWall = 0;
    bool wallSliding = false;

    bool crouch = false;
    bool crouching = false;
    bool canUncrouch = true;

    bool action = false;

    bool diving = false;

    bool bonked = false;
    bool wasBonked = false;
    float bonkTimestamp;

    Controller2D controller;
    InputManager im;
    Animator animator;
    SpriteRenderer sr;

    void Awake() {
        im = new InputManager();
        im.Player.Move.performed += ctx => {
            moveDir = ctx.ReadValue<float>();
            moving = true;
        };
        im.Player.Move.canceled += ctx => {
            moveDir = 0;
            moving = false;
        };
        im.Player.Jump.performed += ctx => {
            if(canJumpAgain) {
                jump = true;
            }
        };
        im.Player.Jump.canceled += ctx => {
            jump = false;
        };
        im.Player.Crouch.performed += ctx => {
            crouch = true;
        };
        im.Player.Crouch.canceled += ctx => {
            crouch = false;
        };
        im.Player.Action.performed += ctx => {
            action = true;
        };
        im.Player.Action.canceled += ctx => {
            action = false;
        };
    }

    void Start() {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        jump = false;
        grounded = false;
    }

    void Update() {
        // Get info from Controller2D
        grounded = controller.isGrounded();
        touchingWall = controller.isTouchingWall();
        falling = actualVelocity.y < 0 && !grounded;
        wallSliding = false;

        // Coyote-time timer
        if(falling && wasGrounded) {
            coyoteTimestamp = Time.time;
        }
        coyote = (Time.time - coyoteTimestamp < coyoteTime);

        // Bonk Timer
        if(diving && touchingWall != 0) {
            bonkTimestamp = Time.time;
        }
        bonked = (Time.time - bonkTimestamp < bonkDuration);
        if(bonked) {moveDir = 0;}

        // Reset Jump
        if(grounded && jumping) {
            canJumpAgain = true;
            jumping = false;
            jumpDir = 0;
        }

        handleCrouching();

        // Calculate Acceleration
        wishVelocity.x = Accelerate(moveDir);
        if(!diving) {
            if(moveDir == -1 && actualVelocity.x < 4) {
                sr.flipX = true;
            }
            if(moveDir == 1 && actualVelocity.x > -4) {
                sr.flipX = false;
            }
        }

        handleGravity();

        // Air Dive
        if(jumping && action && !diving && !wallSliding && moveDir != 0) {
            if(Mathf.Sign(moveDir) != Mathf.Sign(actualVelocity.x)) {
                wishVelocity.x = moveDir * diveForce;
            }
            else {
                wishVelocity.x = (0.25f * wishVelocity.x) + (moveDir * diveForce);
            }
            wishVelocity.y = diveForce * 0.7f;
            diving = true;
        }
        if(diving && grounded) {
            diving = false;
        }

        // Bonk
        if(bonked && !wasBonked) {
            wishVelocity.x = -wishVelocity.x;
            diving = false;
        }

        handleJumping();
        
        // Do actual movement with collisions
        actualVelocity = controller.Move(wishVelocity * Time.deltaTime);

        //setDebugText();

        // Do animation shit
        animator.SetBool("crouching", crouching);
        animator.SetBool("wallSliding", wallSliding);
        animator.SetBool("falling", !grounded);
        animator.SetBool("airDiving", diving);
        animator.SetBool("Bonked", bonked);

        controller.CalculateRaySpacing();

        // Set variables
        wasGrounded = grounded;
        wasBonked = bonked;
    }

    float Accelerate(float wishDir) {
        float speed = actualVelocity.x;
        if(moving) {
            if(Mathf.Abs(speed) < maxSpeed && grounded) {
                speed += wishDir * acceleration / friction;
            }
            else if(!grounded) {
                if(Mathf.Sign(wishDir) != Mathf.Sign(speed)) {
                    speed += wishDir * airAcceleration / friction;
                }
                else if(Mathf.Abs(speed) < maxSpeed) {
                    speed += wishDir * airAcceleration / airFriction;
                }
            }
            if(Mathf.Abs(speed) > maxSpeed) {
                if(grounded) {
                    speed -= Mathf.Sign(speed) * friction * acceleration * Time.deltaTime;
                }
                else {
                    if(Mathf.Sign(wishDir) != Mathf.Sign(jumpDir)) {
                        speed -= Mathf.Sign(speed) * airAcceleration * Time.deltaTime;
                    }
                }
                // speed = grounded ? speed - Mathf.Sign(speed) * friction * acceleration * Time.deltaTime
                //     : speed - Mathf.Sign(speed) * airAcceleration * Time.deltaTime;
            }
        }
        else if(grounded) {
            float decelSpeed = speed - Mathf.Sign(speed) * friction * Time.deltaTime;
            speed = (Mathf.Sign(decelSpeed) != Mathf.Sign(speed)) ? 0 : decelSpeed;
        }
        return speed;
    }

    void handleJumping() {
        // Handle Jumping
        if(jump && (grounded || (coyote && touchingWall == 0)) && canJumpAgain && !crouching && !bonked) {
            wishVelocity.y = jumpForce;
            canJumpAgain = false;
            jump = false;
            jumping = true;
            jumpDir = moveDir;
        }
        // Wall Jump
        else if(jump && canJumpAgain && wallSliding && moving && !crouching) {
            if(moveDir == touchingWall) {
                wishVelocity.y = jumpForce * 0.7f;
                wishVelocity.x = -touchingWall * jumpForce * 0.9f;
                canJumpAgain = false;
                jump = false;
                jumping = true;
                jumpDir = -touchingWall;
            }
        }
    }

    void handleCrouching() {
        // Check crouching stuff
        if(crouching) {
            canUncrouch = controller.canUncrouch();
        }
        if(crouch) {
            if(!crouching) {
                if(grounded) {
                    maxSpeed *= crouchSpeedMult;
                    friction *= crouchFrictionMult;
                    crouching = true;
                    canJumpAgain = false;
                }
                else {
                    // ground pound type thing???
                }
            }
        }
        else {
            if(crouching) {
                if(canUncrouch) {
                    maxSpeed /= crouchSpeedMult;
                    friction /= crouchFrictionMult;
                    crouching = false;
                    canJumpAgain = true;
                }
            }
        }
    }

    void handleGravity() {
        // Handle Gravity
        if(grounded || controller.isBumpingHead()) {
            wishVelocity.y = -gravity * Time.deltaTime;   
        }
        else if(!grounded && touchingWall != 0 && moving && actualVelocity.y < 5) {
            if(actualVelocity.y < -maxWallSlideSpeed) {
                wishVelocity.y += wallSlideFriction * Time.deltaTime;
            }
            else {
                wishVelocity.y -= 2 * gravity * Time.deltaTime * 1/wallSlideFriction;
            }
            wallSliding = true;
            canJumpAgain = true;

            // Anim
            sr.flipX = touchingWall == 1;
        }
        else {
            wishVelocity.y -= gravity * Time.deltaTime;
        }
    }

    void setDebugText() {
        debugText.text = $"HSpeed: {actualVelocity.x.ToString("F2")}\n" +
                        $"VSpeed: {actualVelocity.y.ToString("F2")}\n" +
                        $"Moving: {moving}\n" +
                        $"Grounded: {controller.isGrounded()}\n" +
                        $"Crouching: {crouching}\n" +
                        $"Can Uncrouch: {canUncrouch}\n" +
                        $"Falling: {falling}\n" +
                        $"Coyote: {coyote}\n" +
                        $"Bonked: {bonked}\n" +
                        $"Bumping Head: {controller.isBumpingHead()}\n" +
                        $"Touching Wall: {touchingWall}\n" +
                        $"Wall Sliding: {wallSliding}"
        ;
    }

    void OnEnable() {
        im.Enable();
    }
    void OnDisable() {
        im.Disable();
    }
}
