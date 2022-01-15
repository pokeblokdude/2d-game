using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : PlayerState {

    protected bool DO_MOVEMENT;
    protected bool EXIT_STATE_WHEN_FALLING;
    protected float m_grounded_friction;
    protected float m_grounded_maxSpeed;
    protected float m_grounded_accel;

    float timeOffGround;
    bool coyoteActive;


    public Grounded(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        timeOffGround = 0;
        coyoteActive = false;
        DO_MOVEMENT = true;
        EXIT_STATE_WHEN_FALLING = true;
        m_grounded_friction = playerData.friction;
        m_grounded_maxSpeed = playerData.maxSpeed;
        m_grounded_accel = playerData.acceleration;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        // Check Coyote Time
        if(!grounded && !coyoteActive) {
            timeOffGround = Time.time;
            coyoteActive = true;
        }
        if(Time.time - timeOffGround > playerData.coyoteTime && !grounded && EXIT_STATE_WHEN_FALLING) {
            stateMachine.ChangeState(player.FallingState);
        }

        if(DO_MOVEMENT) {
            player.setVelX(player.Accelerate(moveDir, playerData.acceleration, m_grounded_maxSpeed, m_grounded_friction));
        }
        else {
            player.setVelX(player.Accelerate(0, playerData.acceleration, m_grounded_maxSpeed, m_grounded_friction));
        }
        
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
