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
        rect.DOAnchorPos(Vector2.left * 800, 0.5f).SetEase(Ease.OutCubic);
    }

    public void PlaySceneExit() {
        rect.anchoredPosition = new Vector2(800, 0);
        rect.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
    }

}
