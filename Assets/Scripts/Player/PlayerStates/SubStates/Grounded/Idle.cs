using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grounded {
    
    bool holdingJump;

    public Idle(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
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

        if(actionUp) {
            player.anim.SetBool("lookUp", true);
        }
        else {
            player.anim.SetBool("lookUp", false);
        }

        // State Change Checks
        if(moveDir != 0) {
            stateMachine.ChangeState(player.MovingState);
        }
        if(crouch) {
            stateMachine.ChangeState(player.CrouchingState);
        }
        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.JumpingState);
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
