using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text.RegularExpressions;
using System.Linq;

public class SplineKanjiAnimator {
    private string kanjiDataPath = "Assets/Resources/kanjivgdata/kanjidata_simplified.xml"; //todo: propper path
    private Dictionary<string, List<SplinePath>> kanjiToSplines;

    public SplineKanjiAnimator() {
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.Load(kanjiDataPath);
        var kanjiData = xmlDoc.GetElementsByTagName("kanji");
        kanjiToSplines = new Dictionary<string, List<SplinePath>>();
        foreach (XmlNode kanjiXml in kanjiData) {

            List<SplinePath> splines = new List<SplinePath>();
            var element = kanjiXml.Attributes["element"];

            if (element == null) continue;

            foreach (XmlNode path in kanjiXml) {
                SplinePath splinePath = new SplinePath();
                string separators = @"(?=[A-Za-z])";
                var tokens = Regex.Split(path.InnerText, separators).Where(t => !string.IsNullOrEmpty(t));
                float x = 0f;
                float y = 0f;
                Utils.SVGCommand prevCommand = null;
                foreach (string token in tokens) {
                    Utils.SVGCommand c = Utils.SVGCommand.Parse(token);
                    switch (c.command) {
                        case 'M':
                            x = c.arguments[0];
                            y = c.arguments[1];
                            break;
                        case 'm':
                            x += c.arguments[0];
                            y += c.arguments[1];
                            break;
                        case 's': {
                                float a_x = x;
                                float a_y = y;
                                if (prevCommand != null) {
                                    var lc = prevCommand.command;
                                    if (lc == 'C' || lc == 'S') {
                                        var args = prevCommand.arguments;
                                        a_x = x - args[args.Length - 1 - 3]; //todo ??
                                        a_x = y - args[args.Length - 1 - 2]; //todo ??
                                    }
                                    else if (lc == 's' || lc == 'c') {
                                        var args = prevCommand.arguments;
                                        a_x = x - ((x - args[args.Length - 2]) + args[args.Length - 1 - 3]); //todo ??
                                        a_x = y - ((y - args[args.Length - 1]) + args[args.Length - 1 - 2]); //todo ??
                                    }
                                }
                                for (int i = 0; i < c.arguments.Length; i += 4) {
                                    float b_x = c.arguments[i];
                                    float b_y = c.arguments[i + 1];
                                    float x_2 = c.arguments[i + 2];
                                    float y_2 = c.arguments[i + 3];
                                    splinePath.AddSpline(new Spline(new Vector2(x, y),
                                                                    new Vector2(a_x, a_y),
                                                                    new Vector2(x + b_x, y + b_y),
                                                                    new Vector2(x + x_2, y + y_2)));
                                    a_x = x_2 - (x + b_x); //todo ??
                                    a_x = y_2 - (y + b_y); //todo ??
                                    x += x_2;
                                    y += y_2;
                                }
                            }
                            break;
                        case 'S': {
                                float a_x = x;
                                float a_y = y;
                                if (prevCommand != null) {
                                    var lc = prevCommand.command;
                                    if (lc == 'C' || lc == 'S') {
                                        var args = prevCommand.arguments;
                                        a_x = x - args[args.Length - 1 - 3];
                                        a_x = y - args[args.Length - 1 - 2];
                                    }
                                    else if (lc == 's' || lc == 'c') {
                                        var args = prevCommand.arguments;
                                        a_x = x - ((x - args[args.Length - 2]) + args[args.Length - 1 - 3]);
                                        a_x = y - ((y - args[args.Length - 1]) + args[args.Length - 1 - 2]);
                                    }
                                }
                                for (int i = 0; i < c.arguments.Length; i += 4) {
                                    float b_x = c.arguments[i];
                                    float b_y = c.arguments[i + 1];
                                    float x_2 = c.arguments[i + 2];
                                    float y_2 = c.arguments[i + 3];
                                    splinePath.AddSpline(new Spline(new Vector2(x, y),
                                                                    new Vector2(a_x, a_y),
                                                                    new Vector2(b_x, b_y),
                                                                    new Vector2(x_2, y_2)));
                                    x += x_2;
                                    y += y_2;
                                    a_x = x - b_x;
                                    a_x = y - b_y;
                                }
                            }
                            break;
                        case 'c':
                            for (int i = 0; i < c.arguments.Length; i += 6) {
                                float a_x = c.arguments[i];
                                float a_y = c.arguments[i + 1];
                                float b_x = c.arguments[i + 2];
                                float b_y = c.arguments[i + 3];
                                float x_2 = c.arguments[i + 4];
                                float y_2 = c.arguments[i + 5];
                                splinePath.AddSpline(new Spline(new Vector2(x, y),
                                                                new Vector2(x + a_x, y + a_y),
                                                                new Vector2(x + b_x, y + b_y),
                                                                new Vector2(x + x_2, y + y_2)));
                                x += x_2;
                                y += y_2;
                            }
                            break;
                        case 'C':
                            for (int i = 0; i < c.arguments.Length; i += 6) {
                                float a_x = c.arguments[i];
                                float a_y = c.arguments[i + 1];
                                float b_x = c.arguments[i + 2];
                                float b_y = c.arguments[i + 3];
                                float x_2 = c.arguments[i + 4];
                                float y_2 = c.arguments[i + 5];
                                splinePath.AddSpline(new Spline(new Vector2(x, y),
                                                                new Vector2(a_x, a_y),
                                                                new Vector2(b_x, b_y),
                                                                new Vector2(x_2, y_2)));
                                x = x_2;
                                y = y_2;
                            }
                            break;
                        default:
                            Debug.Log("Bad command: " + c.command);
                            break;
                    }
                    prevCommand = c;
                }
                splines.Add(splinePath);
            }
            if (!kanjiToSplines.ContainsKey(element.Value)) {
                kanjiToSplines.Add(element.Value, splines);
            }
        }
    }

    public GameObject SpawnAnimatedKanji(Vector3 location, Vector3 rotation, string kanji, UnityEngine.Color color) {
        if (!kanjiToSplines.ContainsKey(kanji)) {
            return null;
        }
        
        var go = new GameObject();
        var kanjiObject = go.AddComponent<KanjiObject>();
        kanjiObject.transform.parent = go.transform;
        go.transform.position = location;
        List<LineRenderer> lineRederers = new List<LineRenderer>();
        var paths = kanjiToSplines[kanji];
        var scale = 0.04f;
        int MAX_SPLINE_POINTS = 50;

        List<List<List<Vector2>>> kanjiPaths = new List<List<List<Vector2>>>();
        foreach (var path in paths) {
            List<List<Vector2>> splines = new List<List<Vector2>>();
            foreach (var spline in path.Splines) {
                List<Vector2> line = new List<Vector2>();
                Debug.Log("Max points: " + KanjiObject.MAX_SPLINE_POINTS.ToString());
                for (int i = 0; i < KanjiObject.MAX_SPLINE_POINTS; ++i) {
                    float t = (float)i / (float)(KanjiObject.MAX_SPLINE_POINTS - 1);
                    var point = scale*CalculateCubicBezierPoint(t, spline.A, spline.B, spline.C, spline.D);
                    line.Add(point);
                }
                Debug.Log("Line from sender: " + line.Count.ToString());
                splines.Add(line);
            }
            kanjiPaths.Add(splines);
        }
        kanjiObject.SetPaths(kanjiPaths);
        kanjiObject.SetColor(color);
        kanjiObject.SetInitialized();

        go.transform.Rotate(rotation);// = Quaternion.Euler(rotation);
        return go;
    }

    private Vector2 CalculateCubicBezierPoint(float t, Vector2 p1, Vector2 A, Vector2 B, Vector2 p2) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector2 p = uuu * p1;
        p += 3 * uu * t * A;
        p += 3 * u * tt * B;
        p += ttt * p2;
        return p;
    }

    private System.IO.StreamWriter File;

    private void ParseXmlNode(XmlNode node, ref SplinePath splinePath) {
        foreach (XmlNode child in node) {
            if (child.Name == "g") {
                ParseXmlNode(child, ref splinePath);
            }
            else if (child.Name == "path") {

            }
        }
    }
}
