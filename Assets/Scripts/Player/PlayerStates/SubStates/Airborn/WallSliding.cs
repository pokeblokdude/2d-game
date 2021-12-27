using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSliding : Airborn {

    bool holdingJump;
    float stoppedPressingMoveTimestamp = 0;

    public WallSliding(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        if(jump) {
            holdingJump = true;
        }
        CALCULATE_GRAVITY = false;
    }

    public override void Exit() {
        base.Exit();
        CALCULATE_GRAVITY = true;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        // reset jump
        if(!jump) {
            holdingJump = false;
        }
        
        #region LEDGE GRAB
        GameObject wallObj;
        Collider2D wall;
        Vector2 ledgePoint;
        Vector2[] middles = player.controller.edgeMiddles();
        Vector2[] corners = player.controller.corners();
        // middle point of the hitbox
        Vector2 edge = (touchingWall == -1 ? middles[1] : middles[3]);
        // ledge must be under this point
        Vector2 hand = edge  + new Vector2(0, 0.3f);
        Vector2 clippingPoint = edge - new Vector2(0, 0.1f);
        // and above this point (bottom corner)
        Vector2 corner = (touchingWall == -1 ? corners[0] : corners[3]);
        // if the ledge is between corner and clippingPoint, the player will "clip" the ledge (skip over it) instead of grabbing on
        Debug.DrawRay(hand, Vector2.right * touchingWall * 0.1f, Color.red, Time.deltaTime);
        Debug.DrawRay(corner, Vector2.right * touchingWall * 0.1f, Color.red, Time.deltaTime);
        // raycast to get wall object
        RaycastHit2D hit = Physics2D.Raycast(
            corner,
            Vector2.right * touchingWall,
            0.02f,
            playerData.ledgeGrabLayerMask
        );
        if(hit) {
            wall = hit.collider;
            wallObj = wall.gameObject;
            if(wallObj.CompareTag("ledge")) {
                player.ledgeGrabPoint.enable();
                ledgePoint = touchingWall == -1 ? 
                    new Vector2(wall.bounds.max.x, wall.bounds.max.y) :
                    new Vector2(wall.bounds.min.x, wall.bounds.max.y);
                //Debug.Log("ledge grab");
                player.ledgeGrabPoint.setPosition(ledgePoint);
                // trip over the ledge if high enough up
                if(wall.bounds.max.y < clippingPoint.y) {
                    stateMachine.ChangeState(player.ClippingLedgeState);
                }
                // otherwise grab the ledge and hang on
                else if(wall.bounds.max.y < hand.y) {
                    stateMachine.ChangeState(player.GrabbingLedgeState);
                }
            }
            else {
                player.ledgeGrabPoint.disable();
            }
        }
        #endregion

        if(grounded) {
            stateMachine.ChangeState(player.IdleState);
        }

        if(moveDir != touchingWall && stoppedPressingMoveTimestamp == 0) {
            stoppedPressingMoveTimestamp = Time.time;
        }
        if(moveDir != touchingWall && Time.time - stoppedPressingMoveTimestamp >= playerData.wallStickTime) {
            stoppedPressingMoveTimestamp = 0;
            stateMachine.ChangeState(player.FallingState);
        }

        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.WallJumpingState);
        }

        if(touchingWall == 0) {
            stateMachine.ChangeState(player.FallingState);
        }

        // wall friction
        if(player.actualVelocity.y < -playerData.maxWallSlideSpeed) {
            player.setVelY(player.wishVelocity.y + playerData.wallSlideFriction * Time.deltaTime);
        }
        else {
            player.setVelY(player.wishVelocity.y - 2 * gravity * Time.deltaTime * 1/playerData.wallSlideFriction);
        }

        // sprite flipping
        player.sr.flipX = touchingWall == 1;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
