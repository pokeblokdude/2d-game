using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDivingFreeze : Airborn {

    Vector2 velocity;
    float dir;

    public AirDivingFreeze(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        CALCULATE_GRAVITY = false;
        velocity = player.actualVelocity;
        dir = moveDir;
        //Debug.Log(player.wishVelocity);
    }

    public override void Exit() {
        base.Exit();
        CALCULATE_GRAVITY = true;
        player.setVelX(velocity.x);
        player.setVelY(velocity.y);
        //Debug.Log(player.wishVelocity);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if(Time.time - startTime >= playerData.diveFreezeTime) {
            stateMachine.ChangeState(player.AirDivingState);
        }

        player.setVelX(0);
        player.setVelY(0);

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
