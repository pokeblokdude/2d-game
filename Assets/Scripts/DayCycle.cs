using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCycle : MonoBehaviour {
    
    [SerializeField] int cycleLength = 1200;
    [SerializeField] bool doDaylightCycle = true;
    [Header("Colors")]
    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;
    [SerializeField] Color shiftColor;

    Light2D sun;
    float lastIntensity;

    void Start() {
        sun = GetComponent<Light2D>();
        sun.intensity = 0.5f;
        lastIntensity = sun.intensity;
    }

    void Update() {
        if(doDaylightCycle) {
            sun.intensity = Mathf.Clamp(
                ((Mathf.Sin((2 * Mathf.PI * Time.time) / (float)cycleLength) / 2) + 0.75f),
                0.5f, 1f);
        }

        float intensity = sun.intensity;
        if(intensity < lastIntensity) {
            if(intensity > 0.75f && intensity < 0.85f) {
                sun.color = Color.Lerp(dayColor, shiftColor, Mathf.InverseLerp(0.85f, 0.75f, intensity));
            }
            else if(intensity > 0.65f && intensity < 0.75f) {
                sun.color = Color.Lerp(shiftColor, nightColor, Mathf.InverseLerp(0.75f, 0.65f, intensity));
            }
        }
        else if(intensity > lastIntensity) {
            if(intensity > 0.65f && intensity < 0.75f) {
                sun.color = Color.Lerp(nightColor, shiftColor, Mathf.InverseLerp(0.65f, 0.75f, intensity));
            }
            else if(intensity > 0.75f && intensity < 0.85f) {
                sun.color = Color.Lerp(shiftColor, dayColor, Mathf.InverseLerp(0.75f, 0.85f, intensity));
            }
        }

        lastIntensity = intensity;
    }
}
