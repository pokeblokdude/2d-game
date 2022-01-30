using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] int framerate = 60;
    [SerializeField] bool capFramerate = false;
    [SerializeField][Range(0,1)] float timescale = 1;

    [SerializeField] bool doPlayerSpawnPoint = false;

    Player player;
    SpawnPosition spawnPosition;

    InputManager input;
    bool reset = false;

    void Awake() {
        input = new InputManager();
        input.Game.Reset.performed += ctx => {
            if(!reset) {
                resetGame();
                reset = true;
            }
        };
        input.Game.Reset.canceled += ctx => {
            reset = false;
        };
        input.Game.Quit.performed += ctx => {
            Application.Quit();
        };
    }

    void Start() {
        player = FindObjectOfType<Player>();
        spawnPosition = FindObjectOfType<SpawnPosition>();
        if(doPlayerSpawnPoint) {
            player.transform.position = spawnPosition.transform.position;
        }
    }

    void Update() {
        if(capFramerate) {
            Application.targetFrameRate = framerate;
        }
        Time.timeScale = timescale;
    }

    void resetGame() {
        player.transform.position = spawnPosition.transform.position;
    }

    void OnEnable() {
        input.Enable();
    }

    void OnDisable() {
        input.Disable();
    }

}
