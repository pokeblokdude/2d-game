using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPixelOffset : MonoBehaviour {
    public void setOffset(Vector2 pos) {
        Vector2 pixelOffset = new Vector2(pos.x % 0.0625f, pos.y % 0.0625f);
        transform.localPosition = -pixelOffset;
    }
}
