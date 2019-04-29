using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingManager {

    public Vector3 lastPos;
    public List<List<List<float>>> rawLines; //? public

    private LineRenderer lineRenderer;
    private List<GameObject> lineObjects;

    private int numVertices = 0;

    private List<List<float>> currentRawPoints;
    private Vector3? forward = null;
    private Vector3? right = null;
    private Vector3 planeOrigin;

    //public GameObject drawingPlane;
    //public Text displayText;

    private GameObject lineObject;

    // Start is called before the first frame update
    public DrawingManager() {
        rawLines = new List<List<List<float>>>();
        currentRawPoints = new List<List<float>>();
        lineObjects = new List<GameObject>();
    }

    public void Update(Transform transform) {
        //todo: rework due to inefficency
        lineRenderer.positionCount = numVertices + 1; //?
        lineRenderer.SetPosition(numVertices, transform.position);
        numVertices++;

        if (forward == null || right == null) {
            forward = transform.forward;
            right = transform.right;
            planeOrigin = transform.position;
        }

        Vector3 cross = Vector3.Cross(forward.GetValueOrDefault(), right.GetValueOrDefault());
        Vector3 origin = transform.position - planeOrigin;

        float dist = Vector3.Dot(origin, forward.GetValueOrDefault().normalized);

        Vector3 projectionPoint = transform.position - dist * forward.GetValueOrDefault().normalized;

        float a = Vector3.Dot(projectionPoint, right.GetValueOrDefault().normalized);
        float b = Vector3.Dot(projectionPoint, cross.normalized);

        const float scale = 500;
        currentRawPoints.Add(new List<float>() { scale * a, -scale * b });

        lastPos = transform.position;
    }

    public void InitNewLine() {
        lineObject = new GameObject();
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        //Make it customizable
        lineRenderer.startWidth = .02f;
        lineRenderer.endWidth = .02f;
        lineRenderer.material.SetFloat("_Glossiness", 0.0f);
        lineRenderer.material.color = UnityEngine.Color.red;
        //

        numVertices = 0;
        currentRawPoints.Clear();
    }

    public void EndCurrentLine() {
        rawLines.Add(new List<List<float>>(currentRawPoints));
        lineObjects.Add(lineObject);
    }

    public void ResetAllLines() {
        for (int i = 0; i < lineObjects.Count; ++i) {
            Object.Destroy(lineObjects[i]);
        }
        lineObjects.Clear();
        rawLines.Clear();
        forward = null;
        right = null;
    }
}
