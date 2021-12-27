using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : Airborn {

    float ogGravity;

    public Jumping(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        player.setVelY(playerData.jumpForce);
        ogGravity = gravity;
        gravity = 0;
    }

    public override void Exit() {
        base.Exit();
        gravity = ogGravity;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        
        if(!jump || Time.time - startTime > playerData.jumpIncreaseTime) {
            stateMachine.ChangeState(player.FallingFromJumpState);
        }

        player.setVelX(player.AirAccelerate(moveDir));
        
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
