using UnityEngine;
using UnityEngine.Events;

public class LevelExit : MonoBehaviour {

    public UnityEvent<SceneReference, ExitID> sceneTranstion;
    public SceneReference targetScene;

    public ExitID exitID;
    [SerializeField] ExitID targetExitID;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();
            SceneTransitionData.playerMoveDir = (int)Mathf.Sign(player.actualVelocity.x);
            sceneTranstion.Invoke(targetScene, targetExitID);
        }
    }
}
