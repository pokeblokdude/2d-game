using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    SceneTransitionAnimation anim;

    void Start() {
        anim = FindObjectOfType<SceneTransitionAnimation>();
    }

    public void LoadLevel(SceneReference scene) {
        StartCoroutine(LoadScene(scene));
    }

    IEnumerator LoadScene(SceneReference scene) {
        anim.PlaySceneExit();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

}
