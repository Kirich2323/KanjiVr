using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiLine {

    public KanjiLine() {
        points = new List<Vector2>();
    }

    public void AddPoint(int x, int y) {
        points.Add(new Vector2(x, y));
    }

    public int Count() { return points.Count; }

    public Vector2 GetPoint(int idx) { return points[idx];  }

    private List<Vector2> points;
}
