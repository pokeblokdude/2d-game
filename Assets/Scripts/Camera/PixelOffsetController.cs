using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelOffsetController : MonoBehaviour {

    [SerializeField] PixelOffsetData pixelOffsetData;

    void LateUpdate() {
        transform.localPosition = new Vector3(-pixelOffsetData.pOffsetX, -pixelOffsetData.pOffsetY, 2.5f);
    }
}
