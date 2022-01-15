using UnityEngine;

[CreateAssetMenu(fileName="newPlayerData", menuName="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject {
    
    public float maxFallSpeed = -20;

    [Header("Movement")]
    public float maxSpeed = 5f;
    public float maxAirSpeed = 7f;
    [Range(0, 1)] public float crouchSpeedMult = 0.5f;
    public float acceleration = 1f;
    public float airAcceleration = 2f;
    public float friction = 5f;
    public float crouchFrictionMult = 2f;
    public float airFriction = 30f;
    public float gravity = 5;

    [Header("Jump")]
    public float jumpForce = 10;
    public float jumpIncreaseTime = 0.5f;
    public float coyoteTime = 0.1f;

    [Header("Landing")]
    public float hardLandingThreshold = -10f;

    [Header("Dive")]
    public float diveForce = 5f;
    public Vector2 diveAngle = new Vector2(0.25f, 0.7f);
    public float diveFreezeTime = 0.5f;

    [Header("Rolling")]
    public float rollStopThreshold = 3.5f;

    [Header("Bonk")]
    public float bonkStrengthMult = 1f;
    public float bonkDuration = 1;

    [Header("Wall Slide")]
    public float wallSlideInitSpeed = 4f;
    public float wallSlideFriction = 4f;
    public float maxWallSlideSpeed = 3f;
    public float wallStickTime = 0.2f;
    public float wallJumpForce = 10;
    public Vector2 wallJumpAngle = new Vector2(0.9f, 0.7f);

    [Header("Ledge Grab")]
    public LayerMask ledgeGrabLayerMask;

}
