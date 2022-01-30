using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaning : Grounded {
    
    public Leaning(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        // State Change Checks
        if(moveDir != 0) {
            stateMachine.ChangeState(player.MovingState);
        }
        if(crouch) {
            stateMachine.ChangeState(player.CrouchingState);
        }
        if(jump) {
            stateMachine.ChangeState(player.JumpingState);
        }

        // sprite flipping
        player.sr.flipX = touchingWall == 1;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
