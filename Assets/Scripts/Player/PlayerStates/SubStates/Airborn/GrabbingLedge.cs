using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabbingLedge : Airborn {
    
    float transitionTime = 0.2f;
    float climbDelay = 0.1f;

    public GrabbingLedge(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        CALCULATE_GRAVITY = false;
        DOTween.Init();
        Vector2 hangPoint = player.ledgeGrabPoint.transform.position - new Vector3(0.25f * (touchingWall == 1 ? 1 : -1), 0.75f, 0) ;
        player.transform.DOMove(
            hangPoint,
            transitionTime
        );

        
    }

    public override void Exit() {
        base.Exit();
        CALCULATE_GRAVITY = true;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        player.setVelZero();

        if(Time.time - startTime >= transitionTime + climbDelay) {
            if(moveDir == -1 * touchingWall) {
                stateMachine.ChangeState(player.FallingState);
            }
            else if(crouch) {
                stateMachine.ChangeState(player.WallSlidingNoLedgeGrabState);
            }

            if(moveDir == touchingWall || actionUp || jump) {
                stateMachine.ChangeState(player.ClimbingLedgeState);
            }
        }

        player.sr.flipX = touchingWall != 1;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
