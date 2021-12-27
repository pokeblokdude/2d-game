using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabPoint : MonoBehaviour {

    SpriteRenderer sr;
    Vector3 p;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate() {
        transform.position = p;
    }

    public void setPosition(Vector3 position) {
        p = position;
        transform.position = p;
    }

    public void enable() {
        sr.enabled = true;
    }

    public void disable() {
        sr.enabled = false;
    }
}
