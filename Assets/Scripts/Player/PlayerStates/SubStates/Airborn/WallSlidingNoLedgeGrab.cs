using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidingNoLedgeGrab : Airborn {
    
    bool holdingJump;
    float stoppedPressingMoveTimestamp = 0;

    float ledgeGrabResetTime = 0.5f;

    public WallSlidingNoLedgeGrab(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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
        CALCULATE_GRAVITY = false;
    }

    public override void Exit() {
        base.Exit();
        CALCULATE_GRAVITY = true;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        // reset jump
        if(!jump) {
            holdingJump = false;
        }

        if(Time.time - startTime >= ledgeGrabResetTime) {
            stateMachine.ChangeState(player.WallSlidingState);
        }

        if(grounded) {
            stateMachine.ChangeState(player.IdleState);
        }

        if(moveDir != touchingWall && stoppedPressingMoveTimestamp == 0) {
            stoppedPressingMoveTimestamp = Time.time;
        }
        if(moveDir != touchingWall && Time.time - stoppedPressingMoveTimestamp >= playerData.wallStickTime) {
            stoppedPressingMoveTimestamp = 0;
            stateMachine.ChangeState(player.FallingState);
        }

        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.WallJumpingState);
        }

        if(touchingWall == 0) {
            stateMachine.ChangeState(player.FallingState);
        }

        // wall friction
        if(player.actualVelocity.y < -playerData.maxWallSlideSpeed) {
            player.setVelY(player.wishVelocity.y + playerData.wallSlideFriction * Time.deltaTime);
        }
        else {
            player.setVelY(player.wishVelocity.y - 2 * gravity * Time.deltaTime * 1/playerData.wallSlideFriction);
        }

        // sprite flipping
        player.sr.flipX = touchingWall == 1;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
