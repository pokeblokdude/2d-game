using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPixelOffset : MonoBehaviour {
    
    [SerializeField] PlayerPixelOffset pixelOffset;
    [SerializeField] bool doPixelOffset = false;
    SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        transform.localPosition = Vector2.zero;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 pixelOffset = new Vector2(pos.x % 0.0625f, pos.y % 0.0625f);
        transform.localPosition = -pixelOffset;
    }
}
