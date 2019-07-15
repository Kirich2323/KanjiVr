using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiObject : MonoBehaviour {

    public static int MAX_SPLINE_POINTS = 50;

    private List<LineRenderer> lines;

    private List<List<List<Vector2>>> paths;

    private bool isInitialized = false;
    private bool isRendering = false;
    private bool isAnimationLooped = true;

    private float pathAnimationDuration = 5.0f;
    private List<float> splineLengths = null;
    private int currentDrawingPath = -1;
    private float currentPathTravelledDistance = 0.0f;
    private UnityEngine.Color color;
    private float totalKanjiLength = 0.0f;
    // Start is called before the first frame update
    void Start() {
        lines = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (isInitialized) {
            splineLengths = new List<float>();
            foreach (var path in paths) {
                float length = 0;
                foreach (var spline in path) {
                    for (int i = 0; i < spline.Count - 1; ++i) {
                        float dx = spline[i].x - spline[i + 1].x;
                        float dy = spline[i].y - spline[i + 1].y;
                        length += Mathf.Sqrt(dx * dx + dy * dy);
                    }
                    var ngo = new GameObject();
                    ngo.transform.parent = transform;
                    lines.Add(ngo.AddComponent<LineRenderer>());
                    var line = lines[lines.Count - 1];
                    line.positionCount = 0;
                    line.startWidth = .09f;
                    line.endWidth = .09f;
                    line.material = new Material(Shader.Find("Sprites/Default"));
                    line.material.color = color;
                }
                totalKanjiLength += length;
                splineLengths.Add(length);
            }
            currentDrawingPath = 0;
            isInitialized = false;
            isRendering = true;
        }

        if (isRendering) {
            int c = 0;
            if (currentDrawingPath >= splineLengths.Count) {
                currentDrawingPath = splineLengths.Count - 1;
            }
            float currentDrawingSpeed = totalKanjiLength / pathAnimationDuration;
            //float currentDrawingSpeed = splineLengths[currentDrawingPath] / pathAnimationDuration;
            int p = 0;
            foreach (var path in paths) {
                if (p > currentDrawingPath) {
                    currentDrawingPath = p;
                    currentPathTravelledDistance = 0.0f;
                    break;
                }
                if (p == currentDrawingPath) {
                    currentPathTravelledDistance += currentDrawingSpeed * Time.deltaTime;
                }
                float distance = 0.0f;

                bool isOverflow = false;
                foreach (var spline in path) {
                    var line = lines[c];
                    //var pos = transform.position;
                    var kanjiPoint = new Vector4(spline[0].x, -spline[0].y, 0.0f, 1.0f);
                    Vector3 newKanjiPos = transform.localToWorldMatrix * kanjiPoint;

                    //pos.x += spline[0].x;
                    //pos.y -= spline[0].y;
                    line.positionCount = 1; //bad
                    line.SetPosition(0, newKanjiPos);
                    isOverflow = false;
                    int renderedPoints = 1;
                    for (int i = 1; i < spline.Count; ++i) {
                        float dx = spline[i].x - spline[i - 1].x;
                        float dy = spline[i].y - spline[i - 1].y;
                        float difDistance = Mathf.Sqrt(dx * dx + dy * dy);
                        if (p == currentDrawingPath && distance + difDistance > currentPathTravelledDistance) {
                            isOverflow = true;
                            break;
                        }
                        distance += difDistance;
                        //pos = transform.position;
                        kanjiPoint = new Vector4(spline[i].x, -spline[i].y, 0.0f, 1.0f);
                        newKanjiPos = transform.localToWorldMatrix * kanjiPoint;
                        //pos.x += spline[i].x;
                        //pos.y -= spline[i].y;

                        if (line.positionCount < i + 1) {
                            line.positionCount = i + 1; //bad
                        }
                        line.SetPosition(i, newKanjiPos);
                        renderedPoints += 1;
                    }
                    c++;
                    if (isOverflow) {
                        break;
                    }
                }
                if (isOverflow) {
                    break;
                }
                p++;
            }
            if (p == paths.Count) {
                p = 0;
            }
        }
    }

    public void SetInitialized() {
        isInitialized = true;
    }

    public void SetPaths(List<List<List<Vector2>>> paths) {
        this.paths = paths;
    }

    public void SetIsAnimationLooped(bool val) {
        isAnimationLooped = val;
    }

    public void SetPathAnimationDuration(float time) {
        pathAnimationDuration = time;
    }

    public void SetColor(UnityEngine.Color color) {
        this.color = color;
    }

    float CalculateDrawingSpeedDependingOnItsLength(float length, float time) {
        //todo: make some adjustments to how line speed is calculated depending on it's length
        return 0.0f;
    }

}
