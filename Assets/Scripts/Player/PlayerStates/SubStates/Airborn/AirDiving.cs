using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDiving : Airborn {

    bool firstFrame;
    Vector2 velocity;


    public AirDiving(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        m_airAcceleration /= 2;
        velocity = new Vector2(player.wishVelocity.x, player.wishVelocity.y);
        firstFrame = true;
        
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if(touchingWall != 0) {
            stateMachine.ChangeState(player.BonkedState);
        }

        if(grounded && action) {
            stateMachine.ChangeState(player.RollingState);
        }
        else if(grounded) {
            stateMachine.ChangeState(player.IdleState);
        }
        
        player.setVelX(player.AirAccelerate(moveDir, m_airAcceleration, playerData.maxAirSpeed, playerData.airFriction));

        if(firstFrame) {
            if(Mathf.Sign(moveDir) != Mathf.Sign(velocity.x)) {
                player.setVelX(moveDir * playerData.diveForce);
                player.sr.flipX = !player.sr.flipX;
            }
            else {
                if(Mathf.Abs(velocity.x) > playerData.maxAirSpeed) {
                    player.setVelX(velocity.x + (playerData.diveAngle.x / 2 * moveDir));
                    //Debug.Log("vel higher than max");
                }
                else {
                    player.setVelX((playerData.diveAngle.x * velocity.x) + (moveDir * playerData.diveForce));
                    //Debug.Log("vel lower than max");
                }
                
            }
            player.setVelY(playerData.diveForce * playerData.diveAngle.y);

            // sprite flipping
            if(moveDir == -1) {
                player.sr.flipX = true;
            }
            if(moveDir == 1) {
                player.sr.flipX = false;
            }

            firstFrame = false;
        }

        
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
