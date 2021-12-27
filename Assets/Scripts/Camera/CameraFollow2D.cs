using System;
using UnityEngine;

// FROM UNITY STANDARD 2D ASSETS
public class CameraFollow2D : MonoBehaviour {

    [SerializeField] PixelOffsetData pixelOffsetData;
    [SerializeField] bool doPixelOffset = true;

    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    public float m_OffsetZ;
    
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    float pixelOffsetX;
    float pixelOffsetY;

    void Start() {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }

    void Update() {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget) {
            m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
        }
        else {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        // pixel perfect shit
        pixelOffsetX = newPos.x % 0.0625f;
        pixelOffsetY = newPos.y % 0.0625f;
        pixelOffsetData.pOffsetX = pixelOffsetX;
        pixelOffsetData.pOffsetY = pixelOffsetY;
        if(doPixelOffset) {
            newPos = new Vector3(newPos.x - pixelOffsetX, newPos.y - pixelOffsetY, -10);
        }

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }
}