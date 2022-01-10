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
