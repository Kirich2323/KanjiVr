using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radical {
    public Radical(string RadicalName = "") {
        Splines = new List<Spline>();
        Name = RadicalName;
    }

    public void AddSpline(Spline NewSpline) {
        Splines.Add(NewSpline);
    }

    public List<Spline> Splines {
        get; private set;
    }

    public string Name {
        get;  private set;
    }
}
