using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : Airborn {

    public Falling(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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

        if(touchingWall != 0) {
            // go to wall sliding state
            if(moveDir == touchingWall && player.actualVelocity.y < playerData.wallSlideInitSpeed) {
                if(player.actualVelocity.y < 1) {
                    stateMachine.ChangeState(player.WallSlidingNoLedgeGrabState);
                }
                else {
                    stateMachine.ChangeState(player.WallSlidingState);
                }
                
            }
        }
        

        player.setVelX(player.AirAccelerate(moveDir));

        // sprite flipping
        if(moveDir == -1 && player.actualVelocity.x < 4) {
            player.sr.flipX = true;
        }
        if(moveDir == 1 && player.actualVelocity.x > -4) {
            player.sr.flipX = false;
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
