using UnityEngine;

public class Persistent : MonoBehaviour {

    public string objectID;

    void Awake() {
        objectID = name + transform.position.ToString();
    }

    void Start() {
        Persistent[] objects = FindObjectsOfType<Persistent>();
        for(int i = 0; i < objects.Length; i++) {
            if(objects[i] != this) {
                if(objects[i].objectID == objectID) {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
