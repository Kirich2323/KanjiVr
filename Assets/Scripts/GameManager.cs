using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private DrawAnimatedKanji tmp;
    private SplineKanjiAnimator tmp2;
    void Start() {
        //tmp = new DrawAnimatedKanji();
        //tmp.SpawnAnimatedKanji(new Vector3(5, 7, 0), new Vector3(), "義");

        tmp2 = new SplineKanjiAnimator();
        //tmp2.SpawnAnimatedKanji(new Vector3(8f, 6, 43.79f), new Vector3(0, 90f, 0), "木", UnityEngine.Color.green);
        //tmp2.SpawnAnimatedKanji(new Vector3(-10.51f, 7f, 47.25f), new Vector3(0, 90f, 0), "火", UnityEngine.Color.red);
        tmp2.SpawnAnimatedKanji(new Vector3(2.02f, 12f, 7.544f), new Vector3(0, 90f, 0), "義", UnityEngine.Color.blue);
        tmp2.SpawnAnimatedKanji(new Vector3(-0.57f, 4.69f, 27.42f), new Vector3(0, 90f, 0), "地", UnityEngine.Color.black);
        tmp2.SpawnAnimatedKanji(new Vector3(-2.12f, 4.69f, -47.78f), new Vector3(0, 90f, 0), "草", UnityEngine.Color.green);
    }

    void Update() {

    }
}
