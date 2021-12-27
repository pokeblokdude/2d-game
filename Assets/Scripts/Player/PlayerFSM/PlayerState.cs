using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected InputManager input;

    protected float startTime;

    private string animBoolName;

    protected bool CALCULATE_GRAVITY = true;

    protected float gravity;
    protected bool grounded;
    protected float moveDir = 0;
    protected bool crouch = false;
    protected bool jump = false;
    protected bool action = false;
    protected bool actionUp = false;
    protected int touchingWall;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        DoChecks();
        player.anim.SetBool(animBoolName, true);
        startTime = Time.time;
        //Debug.Log(animBoolName);

        gravity = playerData.gravity;

        player.ledgeGrabPoint.disable();
    }

    public virtual void Exit() {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() {
        DoChecks();
    }

    public virtual void PhysicsUpdate() {
        
    }

    public virtual void DoChecks() {
        grounded = player.controller.isGrounded();
        moveDir = player.input.moveDir;
        crouch = player.input.crouch;
        jump = player.input.jump;
        action = player.input.action;
        actionUp = player.input.actionUp;
        touchingWall = player.controller.isTouchingWall();

        // GRAVITY
        if(CALCULATE_GRAVITY) {
            if(grounded || player.controller.isBumpingHead()) {
                player.setVelY(-playerData.gravity * Time.deltaTime);
            }
            else if(player.actualVelocity.y >= playerData.maxFallSpeed) {
                player.setVelY(player.wishVelocity.y - (gravity * Time.deltaTime));
            }
        }
    }

    public string Name() {
        return animBoolName;
    }

    // DEBUGGING
    public bool calculatingGravity() {
        return CALCULATE_GRAVITY;
    }
}  
