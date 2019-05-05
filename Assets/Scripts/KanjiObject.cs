using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiObject : MonoBehaviour {

    public static int MAX_SPLINE_POINTS = 50;

    private List<LineRenderer> lines;

    private List<List<List<Vector2>>> paths;

    private bool isInitialized = false;
    private bool isRendering = false;
    // Start is called before the first frame update
    void Start() {
        lines = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(transform.right.ToString() + " -- " + transform.up.ToString());
        if (isInitialized) {
            foreach (var path in paths) {
                foreach (var spline in path) {
                    var ngo = new GameObject();
                    ngo.transform.parent = transform;
                    lines.Add(ngo.AddComponent<LineRenderer>());
                    var line = lines[lines.Count - 1];
                    line.positionCount = spline.Count;
                    line.startWidth = .09f;
                    line.endWidth = .09f;
                    line.material = new Material(Shader.Find("Sprites/Default"));
                    line.material.color = UnityEngine.Color.red;
                }
            }
            isInitialized = false;
            isRendering = true;
        }

        if (isRendering) {
            int c = 0;
            foreach (var path in paths) {
                foreach (var spline in path) {
                    var line = lines[c];
                    for (int i = 0; i < spline.Count; ++i) {
                        var pos = transform.position;
                        var vx = new Vector3(spline[i].x, 0, 0);
                        var vy = new Vector3(0, -spline[i].y, 0);
                        //pos += Vector3.Scale(vx, transform.forward) + Vector3.Scale(vy, transform.up);
                        pos.x += spline[i].x;
                        pos.y -= spline[i].y;
                        //pos *= Quaternion.Euler(transform.rotation.eulerAngles);
                        line.SetPosition(i, pos);
                    }
                    c++;
                }
            }
        }
    }

    public void SetInitialized() {
        isInitialized = true;
    }

    public void SetPaths(List<List<List<Vector2>>> paths) {
        this.paths = paths;
    }

}
