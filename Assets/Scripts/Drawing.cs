using System.Drawing;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using BitmapDrawrer;
using Tesseract;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class Drawing : MonoBehaviour {

    private VRTK_ControllerEvents controllerEvents;

    private LineRenderer currentLine;
    private List<GameObject> currentLines;
    private Vector3 lastPos;
    private int numVertices = 0;

    private List<List<List<float>>> lines;
    private List<List<float>> currentPoints;
    private Vector3? forward = null;
    private Vector3? right = null;
    private Vector3 planeOrigin;

    public GameObject drawingPlane;
    public Text displayText;

    private TesseractEngine engine;

    private IntPtr recognizer;
    private IntPtr zinniaCharacter;

    private GameObject lineObject;

    void Start() {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.TriggerPressed += DoTriggerPressed;
        controllerEvents.TriggerReleased += DoTriggerReleased;
        controllerEvents.GripPressed += DoGripPressed;

        zinnia_version();

        lines = new List<List<List<float>>>();
        currentPoints = new List<List<float>>();
        currentLines = new List<GameObject>();

        engine = new TesseractEngine(@"./Assets/Resources/tessdata", "jpn", EngineMode.Default); //todo: propper path

        //Zinnia
        recognizer = zinnia_recognizer_new();
        zinnia_recognizer_open(recognizer, Encoding.ASCII.GetBytes(Application.dataPath+ @"\Resources\zinniadata\handwriting-ja.model")); //todo: propper path
        zinniaCharacter = zinnia_character_new();
        //Zinnie
    }

    private static string PtrToStringUtf8(IntPtr ptr) // aPtr is nul-terminated
{
        if (ptr == IntPtr.Zero)
            return "";
        int len = 0;
        while (System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0)
            len++;
        if (len == 0)
            return "";
        byte[] array = new byte[len];
        System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
        return System.Text.Encoding.UTF8.GetString(array);
    }

    void FixedUpdate() {

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
        lineObject = new GameObject();
        currentLine = lineObject.AddComponent<LineRenderer>();
        currentLine.startWidth = .02f;
        currentLine.endWidth = .02f;
        currentLine.material.SetFloat("_Glossiness", 0.0f);
        currentLine.material.color = UnityEngine.Color.red;
        numVertices = 0;
        currentPoints.Clear();
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e) {
        lines.Add(new List<List<float>>(currentPoints));
        currentLines.Add(lineObject);
        Debug.Log("Released");
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e) {
        int margin = 50;

        //var recognized = TesseractRegonize(margin);
        var recognized = ZinniaRecognize(margin);
        
        displayText.text = recognized;

        for (int i = 0; i < currentLines.Count; ++i) {
            Destroy(currentLines[i]);
        }
        currentLines.Clear();
        lines.Clear();
        forward = null;
        right = null;
    }

    private static float Normalize(float current, float min, float max) {
        return (current - min) / (max - min);
    }

    string TesseractRegonize(int margin) {

        if (lines.Count < 1) {
            return "";
        }

        for (int i = 0; i < lines.Count; ++i) {
            if (lines[i].Count < 1) {
                return "";
            }
        }

        var bmap = BitmapDrawrer.Drawrer.DrawLines(lines, margin);

        bmap.Save("debug_image.png");

        GraphicsUnit units = GraphicsUnit.Pixel;
        var bounds = bmap.GetBounds(ref units);
        var texture = new Texture2D((int)bounds.Width, (int)bounds.Height, TextureFormat.RGBA32, false);
        var bytes = BitmapDrawrer.Drawrer.imageToByteArray(bmap);

        texture.LoadRawTextureData(bytes);
        texture.Apply();

        var renderer = drawingPlane.GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
        renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        texture.Apply();

        //Tesseract calls
        engine.DefaultPageSegMode = PageSegMode.SingleChar;
        Page page = engine.Process(bmap);
        var text = page.GetText();
        var confidence = page.GetMeanConfidence();
        page.Dispose();
        //
        return text;
    }

    string ZinniaRecognize(int margin) {

        Debug.Log("Total lines: " + lines.Count.ToString());
        for (int i = 0; i < lines.Count; ++i) {
            Debug.Log("line " + i.ToString() + "   "  + lines[i].Count.ToString());
            for (int j = 0; j < lines[i].Count; ++j) {
                Debug.Log(lines[i][j][0].ToString() + "  " + lines[i][j][1].ToString());
            }
        }

        if (lines.Count < 1) {
            return "";
        }
        for (int i = 0; i < lines.Count; ++i) {
            if (lines[i].Count < 1) {
                return "";
            }
        }

        zinnia_character_clear(zinniaCharacter);
        float minX = 1e10f;
        float maxX = -1e10f;
        float minY = 1e10f;
        float maxY = -1e10f;

        for (int i = 0; i < lines.Count; ++i) {
            for (int j = 0; j < lines[i].Count; ++j) {
                if (minX > lines[i][j][0]) {
                    minX = lines[i][j][0];
                }
                if (maxX < lines[i][j][0]) {
                    maxX = lines[i][j][0];
                }
                if (minY > lines[i][j][1]) {
                    minY = lines[i][j][1];
                }
                if (maxY > lines[i][j][1]) {
                    maxY = lines[i][j][1];
                }
            }
        }

        float width = maxX - minX + margin * 2;
        float height = maxY - minY + margin * 2;

        //zinnia_character_set_width(zinniaCharacter, (uint)width);   //breaks recognition
        //zinnia_character_set_height(zinniaCharacter, (uint)height); //breaks recognition

        for (int i = 0; i < lines.Count; ++i) {
            for (int j = 0; j < lines[i].Count; ++j) {
                zinnia_character_add(zinniaCharacter, (uint)i, (int)(lines[i][j][0] - minX + margin), (int)(lines[i][j][1] - minY + margin)); //todo: maybe math.round
            }
        }

        int topn = 5;
        var result = zinnia_recognizer_classify(recognizer, zinniaCharacter, (uint)topn);
        string ans = "";
        for (int i = 0; i < topn; ++i) {
            ans += PtrToStringUtf8(zinnia_result_value(result, (uint)i)) + " ";
        }
        zinnia_result_destroy(result);
        return ans;
    }


    void OnDestroy() {
        zinnia_character_destroy(zinniaCharacter);
        zinnia_recognizer_destroy(recognizer);
    }

    // Character
    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static IntPtr zinnia_version();

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static IntPtr zinnia_character_new();

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void zinnia_character_destroy(IntPtr character);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int zinnia_character_add(IntPtr character, uint id, int x, int y);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static uint zinnia_character_width(IntPtr character);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static uint zinnia_character_height(IntPtr character);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void zinnia_character_set_width(IntPtr character, uint width);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void zinnia_character_set_height(IntPtr character, uint height);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void zinnia_character_clear(IntPtr character);


    // Result
    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static IntPtr zinnia_result_value(IntPtr result, uint i);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void zinnia_result_destroy(IntPtr result);


    //Recognizer
    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static IntPtr zinnia_recognizer_new();

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void zinnia_recognizer_destroy(IntPtr recognizer);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static IntPtr zinnia_recognizer_open(IntPtr recognizer, byte[] filename);

    [DllImport("libzinnia.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static IntPtr zinnia_recognizer_classify(IntPtr recognizer, IntPtr character, uint nbest);
}