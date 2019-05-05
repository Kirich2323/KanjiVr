using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePath {
    public SplinePath() {
        Splines = new List<Spline>();
    }

    public void AddSpline(Spline spline) {
        Splines.Add(spline);
    }

    public List<Spline> Splines {
        get; private set;
    }

}
