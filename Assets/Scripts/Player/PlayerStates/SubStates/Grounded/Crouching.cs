using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : Grounded {

    public Crouching(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        m_grounded_friction *= playerData.crouchFrictionMult;
        m_grounded_maxSpeed *= playerData.crouchSpeedMult;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if(!crouch && player.controller.canUncrouch()) {
            stateMachine.ChangeState(player.IdleState);
        }
        if(moveDir != 0) {
            stateMachine.ChangeState(player.CrouchingMovingState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
