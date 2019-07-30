using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    // Start is called before the first frame update
    private SplineKanjiAnimator spawner;

    void Start() {
        spawner = new SplineKanjiAnimator();

        spawner.SpawnAnimatedKanji(new Vector3(0f, 4f, 5f), new Vector3(0, 90f, 0), "義", UnityEngine.Color.blue);
        spawner.SpawnAnimatedKanji(new Vector3(0f, 4f, 10f), new Vector3(0, 90f, 0), "地", UnityEngine.Color.black);
        spawner.SpawnAnimatedKanji(new Vector3(0f, 4f, 15), new Vector3(0, 90f, 0), "草", UnityEngine.Color.green);
    }

    // Update is called once per frame
    void Update() {

    }
}
