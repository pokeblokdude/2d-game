using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : Grounded {
    
    bool holdingJump;

    public Running(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
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
        m_grounded_friction /= 5;
        m_grounded_accel = 0.01f;
        //DO_MOVEMENT = false;
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

        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.JumpingState);
        }

        if(Time.time - startTime >= 2 || Mathf.Abs(player.actualVelocity.x) < m_grounded_maxSpeed/*|| (moveDir != Mathf.Sign(player.actualVelocity.x) && moveDir != 0)*/) {
            stateMachine.ChangeState(player.MovingState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
