using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborn : PlayerState {

    protected float m_airAcceleration;

    public Airborn(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        m_airAcceleration = playerData.airAcceleration;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
