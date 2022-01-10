using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFromJump : Airborn {

    public FallingFromJump(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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
        if(player.controller.isGrounded()) {
            if(player.wishVelocity.y < playerData.hardLandingThreshold) {
                stateMachine.ChangeState(player.HardLandingState);
            }
            else {
                if(moveDir == 0) {
                    stateMachine.ChangeState(player.IdleState);
                }
                else {
                    stateMachine.ChangeState(player.MovingState);
                }
            }
        }

        base.LogicUpdate();

        if(touchingWall != 0 && moveDir == touchingWall && player.actualVelocity.y < playerData.wallSlideInitSpeed) {
            stateMachine.ChangeState(player.WallSlidingState);
        }

        if(action && moveDir != 0 && touchingWall == 0) {
            stateMachine.ChangeState(player.AirDivingFreezeState);
        }

        player.setVelX(player.AirAccelerate(moveDir, m_airAcceleration, playerData.maxAirSpeed, playerData.airFriction));

        // sprite flipping
        if(moveDir == -1 && player.actualVelocity.x < 3) {
            player.sr.flipX = true;
        }
        if(moveDir == 1 && player.actualVelocity.x > -3) {
            player.sr.flipX = false;
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
