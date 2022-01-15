using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : Grounded {
    
    bool holdingJump;

    public Rolling(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        if(jump) {
            holdingJump = true;
        }
        EXIT_STATE_WHEN_FALLING = false;
        m_grounded_friction /= 10;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        // reset jump
        if(!jump) {
            holdingJump = false;
        }
        
        DO_MOVEMENT = !(moveDir == Mathf.Sign(player.actualVelocity.x));

        // State Change Checks
        bool canUncrouch = player.controller.canUncrouch();
        if(!action && canUncrouch && Mathf.Abs(player.actualVelocity.x) >= m_grounded_maxSpeed) {
            stateMachine.ChangeState(player.RunningState);
        }
        else if(!action && canUncrouch && Mathf.Abs(player.actualVelocity.x) < m_grounded_maxSpeed) {
            stateMachine.ChangeState(player.MovingState);
        }
        else if(!action && !canUncrouch) {
            stateMachine.ChangeState(player.CrouchingMovingState);
        }
        if(Mathf.Abs(player.actualVelocity.x) < playerData.rollStopThreshold && canUncrouch) {
            stateMachine.ChangeState(player.MovingState);
        }
        else if (Mathf.Abs(player.actualVelocity.x) < playerData.rollStopThreshold && !canUncrouch) {
            stateMachine.ChangeState(player.CrouchingMovingState);
        }
        if(jump && !holdingJump) {
            //stateMachine.ChangeState(player.JumpingState);
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
