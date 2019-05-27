using UnityEngine;
using VRTK;
using Tesseract;
using UnityEngine.UI;

public class VrtkDrawing : MonoBehaviour {

    private VRTK_ControllerEvents controllerEvents;

    public GameObject drawingPlane;
    public Text displayText;

    private TesseractEngine engine;

    private DrawingManager drawingManager;
    private ZinniaRecognition zinniaRecognition;

    void Start() {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.TriggerPressed += DoTriggerPressed;
        controllerEvents.TriggerReleased += DoTriggerReleased;
        controllerEvents.GripPressed += DoGripPressed;

        drawingManager = new DrawingManager();
        zinniaRecognition = new ZinniaRecognition();

        //engine = new TesseractEngine(@"./Assets/Resources/tessdata", "jpn", EngineMode.Default); //todo: propper path
    }

    void Update() {

        if (transform.position == drawingManager.lastPos) {
            return;
        }

        if (controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerPress)) {
            drawingManager.Update(transform);
        }
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e) {
        drawingManager.InitNewLine();
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e) {
        drawingManager.EndCurrentLine();
        Debug.Log("Released");
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e) {
        int margin = 50;

        //var recognized = TesseractRegonize(margin);

        var recognized = zinniaRecognition.Recognize(ref drawingManager.rawLines, margin);
        displayText.text = recognized;

        drawingManager.ResetAllLines();
    }

    //private static float Normalize(float current, float min, float max) {
    //    return (current - min) / (max - min);
    //}

    //string TesseractRegonize(int margin) {
    //    if (lines.Count < 1) {
    //        return "";
    //    }

    //    for (int i = 0; i < lines.Count; ++i) {
    //        if (lines[i].Count < 1) {
    //            return "";
    //        }
    //    }

    //    var bmap = BitmapDrawrer.Drawrer.DrawLines(lines, margin);
    //    bmap.Save("debug_image.png");

    //    GraphicsUnit units = GraphicsUnit.Pixel;
    //    var bounds = bmap.GetBounds(ref units);
    //    var texture = new Texture2D((int)bounds.Width, (int)bounds.Height, TextureFormat.RGBA32, false);
    //    var bytes = BitmapDrawrer.Drawrer.imageToByteArray(bmap);

    //    texture.LoadRawTextureData(bytes);
    //    texture.Apply();

    //    var renderer = drawingPlane.GetComponent<Renderer>();
    //    renderer.material.mainTexture = texture;
    //    renderer.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
    //    texture.Apply();

    //    //Tesseract calls
    //    Debug.Log("Calling tesseract");
    //    engine.DefaultPageSegMode = PageSegMode.SingleChar;
    //    Debug.Log("Page mode selected");
    //    Page page = engine.Process(bmap);
    //    Debug.Log("Page is obtained");
    //    var text = page.GetText();
    //    Debug.Log("Text is parsed");
    //    var confidence = page.GetMeanConfidence();
    //    Debug.Log("Got the confidence");
    //    page.Dispose();
    //    Debug.Log("Page is disposed");
    //    //
    //    return text;
    //}

    void OnDestroy() {        
    }

    void OnApplicationQuit() {
    }
}