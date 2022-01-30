using System;
using UnityEngine;

public class PixelPerfectCameraFollow : MonoBehaviour {

    public Transform target;

    void LateUpdate() {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}