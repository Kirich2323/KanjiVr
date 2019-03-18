using System.Drawing;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using BitmapDrawrer;
using Tesseract;
using UnityEngine.UI;

public class Drawing : MonoBehaviour {

    private VRTK_ControllerEvents controllerEvents;
    private LineRenderer currentLine;
    private List<LineRenderer> currentLines;
private Vector3 lastPos;
    private int numVertices = 0;

    private List<List<List<float>>> lines;
    private List<List<float>> currentPoints;
    private Vector3? forward = null;
    private Vector3? right = null;
    private Vector3 planeOrigin;

    public GameObject drawingPlane;
    public Text displayText;

    void Start() {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        controllerEvents.TriggerPressed += DoTriggerPressed;
        controllerEvents.TriggerReleased += DoTriggerReleased;
        controllerEvents.GripPressed += DoGripPressed;
        lines = new List<List<List<float>>>();
        currentPoints = new List<List<float>>();
        currentLines = new List<LineRenderer>();
    }

    void Update() {

        if (transform.position == lastPos) {
            return;
        }

        if (controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerPress)) {

            //todo: rework due to inefficency
            currentLine.positionCount = numVertices + 1; //?
            currentLine.SetPosition(numVertices, transform.position);
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
            currentPoints.Add(new List<float>() { scale * a, -scale * b });

            Debug.Log("pressed");
        }
        else {
            //Debug.Log("Not pressed");
            //Debug.Log(transform.position);
        }
        lastPos = transform.position;

    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e) {
        GameObject go = new GameObject();
        currentLine = go.AddComponent<LineRenderer>();
        currentLine.startWidth = .02f;
        currentLine.endWidth = .02f;
        currentLine.material.SetFloat("_Glossiness", 0.0f);
        currentLine.material.color = UnityEngine.Color.red;
        numVertices = 0;

        currentPoints.Clear();
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e) {
        lines.Add(new List<List<float>>(currentPoints));
        currentLines.Add(currentLine);
        Debug.Log("Released");
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e) {
        int margin = 50;
        var bmap = BitmapDrawrer.Drawrer.DrawLines(lines, margin);

        for (int i = 0; i < currentLines.Count; ++i) {
            Destroy(currentLines[i]);
        }
        lines.Clear();
        //bmap.Save("test.png");

        GraphicsUnit units = GraphicsUnit.Pixel;
        var bounds = bmap.GetBounds(ref units);
        var texture = new Texture2D((int)bounds.Width, (int)bounds.Height, TextureFormat.RGBA32, false);
        var bytes = BitmapDrawrer.Drawrer.imageToByteArray(bmap);
        Debug.Log(bytes.Length);
        texture.LoadRawTextureData(bytes);
        texture.Apply();

        var renderer = drawingPlane.GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
        renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        texture.Apply();

        //Tesseract calls
        var engine = new TesseractEngine(@"./Assets/tessdata", "jpn", EngineMode.Default);
        engine.DefaultPageSegMode = PageSegMode.SingleChar;
        Page page = engine.Process(bmap);
        var text = page.GetText();
        var confidence = page.GetMeanConfidence();
        displayText.text = "Recognized kanji: " + text + "Confidence: " + confidence.ToString();
        Debug.Log("Parsed kanji: " + text);
        Debug.Log("Confidence: " + confidence.ToString());
        Debug.Log("Engine is created");
        //
    }

    private static float Normalize(float current, float min, float max) {
        return (current - min) / (max - min);
    }
}