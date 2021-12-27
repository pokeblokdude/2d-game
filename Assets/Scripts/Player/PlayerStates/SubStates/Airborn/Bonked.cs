using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonked : Airborn {
    
    public Bonked(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        Vector2 velocity = new Vector2(player.wishVelocity.x, player.wishVelocity.y);
        player.setVelZero();
        player.setVelActual(-velocity.x * playerData.bonkStrengthMult, 0);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        
        if(Time.time - startTime >= playerData.bonkDuration) {
            if(grounded) {
                if(moveDir != 0) {
                    stateMachine.ChangeState(player.MovingState);
                }
                else {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
            else {
                stateMachine.ChangeState(player.FallingState);
            }
        }

        if(grounded) {
            player.setVelX(player.Accelerate(0, playerData.acceleration, playerData.maxSpeed, playerData.friction));
        }
        else {
            player.setVelX(player.AirAccelerate(0));
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
