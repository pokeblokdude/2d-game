using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransitionAnimation : MonoBehaviour {
    
    [SerializeField] RectTransform rect;

    void Start() {
        PlaySceneEnter();
    }

    public void PlaySceneEnter() {
        rect.DOAnchorPos(Vector2.left * 800, 1);
    }

    public void PlaySceneExit() {
        rect.DOAnchorPos(Vector2.zero, 1);
    }

}
