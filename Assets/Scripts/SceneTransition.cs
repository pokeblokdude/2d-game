using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public UnityEvent<SceneReference> sceneTranstion;
    public SceneReference targetScene;

    void OnTriggerEnter2D(Collider2D other) {
        print("collision");
        if(other.gameObject.CompareTag("Player")) {
            sceneTranstion.Invoke(targetScene);
        }
    }
}
