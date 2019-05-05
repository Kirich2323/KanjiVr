using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private DrawAnimatedKanji tmp;
    private SplineKanjiAnimator tmp2;
    void Start() {
        tmp = new DrawAnimatedKanji();
        tmp.SpawnAnimatedKanji(new Vector3(5, 7, 0), new Vector3(), "義");

        tmp2 = new SplineKanjiAnimator();
        tmp2.SpawnAnimatedKanji(new Vector3(0, 7, 0), new Vector3(0, 90f, 0), "義");
    }

    void Update() {

    }
}
