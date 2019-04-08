using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiLine {

    public KanjiLine() {
        points = new List<Vector2>();
    }

    public void addPoint(int x, int y) {
        points.Add(new Vector2(x, y));
    }

    private List<Vector2> points;
}
