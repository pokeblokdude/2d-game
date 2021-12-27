using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumping : Airborn {

    public WallJumping(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        player.setVelY(playerData.wallJumpForce * playerData.wallJumpAngle.y);
        player.setVelX(-touchingWall * playerData.wallJumpForce * playerData.wallJumpAngle.x);
        //player.sr.flipX = touchingWall == 1 ? true : false;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        stateMachine.ChangeState(player.FallingFromJumpState);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
