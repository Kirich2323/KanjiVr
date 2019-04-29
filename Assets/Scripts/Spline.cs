using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline {

    public Spline(Vector2 A, Vector2 B, Vector2 C, Vector2 D) {
        this.A = A;
        this.B = B;
        this.C = C;
        this.D = D;
    }

    public Vector2 A {
        get; private set;
    }

    public Vector2 B {
        get; private set;
    }

    public Vector2 C {
        get; private set;
    }

    public Vector2 D {
        get; private set;
    }

}
