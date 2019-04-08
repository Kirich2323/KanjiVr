using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private DrawAnimatedKanji tmp;
    void Start() {
        tmp = new DrawAnimatedKanji();
        tmp.SpawnAnimatedKanji(new Vector3(0, 5, 0), new Vector3(), "義");
    }

    void Update() {

    }
}
