using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClimbingLedge : Airborn {
    
    float transitionTime = 0.4f;

    public ClimbingLedge(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        CALCULATE_GRAVITY = false;
        player.CALCULATE_COLLISION = false;
        DOTween.Init();
        Vector2 endPoint = player.ledgeGrabPoint.transform.position + new Vector3(0.25f * (touchingWall == 1 ? 1 : -1), 0, 0);
        player.transform.DOMove(
            endPoint,
            transitionTime
        );
    }

    public override void Exit() {
        base.Exit();
        CALCULATE_GRAVITY = true;
        player.CALCULATE_COLLISION = true;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        player.setVelZero();

        if(Time.time - startTime >= transitionTime) {
            stateMachine.ChangeState(player.IdleState);
        }
        player.sr.flipX = touchingWall != 1;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
