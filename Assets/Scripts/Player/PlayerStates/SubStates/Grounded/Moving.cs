using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Grounded {
    
    bool holdingJump;
    
    public Moving(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
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

        if(crouch) {
            stateMachine.ChangeState(player.CrouchingMovingState);
        }

        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.JumpingState);
        }

        if(player.actualVelocity.x == 0) {
            stateMachine.ChangeState(player.IdleState);
        }

        // sprite flipping
        if(moveDir == -1) {
            player.sr.flipX = true;
        }
        if(moveDir == 1) {
            player.sr.flipX = false;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
