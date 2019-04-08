using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
//using System.Xml.Serialization;

public class DrawAnimatedKanji {
    private string kanjiData = "Assets/Resources/zinniadata/handwriting-ja.xml"; //todo: propper path

    Dictionary<string, KanjiLines> dict;

    public DrawAnimatedKanji() {
        Debug.Log("Constructor");
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        //xmlDoc.LoadXml(kanjiData); // load the file.
        xmlDoc.Load(kanjiData);
        var kanjis = xmlDoc.GetElementsByTagName("character");

        dict = new Dictionary<string, KanjiLines>();
        int c = 0;
        int collisions = 0;
        foreach (XmlNode kanjiXml in kanjis) {
            Debug.Log(c);
            c++;
            var lines = new KanjiLines();
            var kanjiChar = "";
            foreach (XmlNode node in kanjiXml.ChildNodes) {
                if (node.Name == "utf8") {
                    kanjiChar = node.InnerText;
                    //Debug.Log(kanjiChar);
                    //return;
                }
                else if (node.Name == "strokes") {
                    foreach (XmlNode stroke in node) {
                        if (stroke.Name != "stroke") continue;

                        var line = new KanjiLine();
                        foreach (XmlNode point in stroke.ChildNodes) {
                            if (point.Name != "point") continue;
                            line.AddPoint(int.Parse(point.Attributes["x"].Value),
                                          int.Parse(point.Attributes["y"].Value));
                        }
                        lines.AddLine(line);
                    }
                }
            }

            if (!dict.ContainsKey(kanjiChar)) {
                dict.Add(kanjiChar, lines);
            }
            else {
                collisions++;
            }
        }
        Debug.Log("Collisions in xml data: " + collisions.ToString());
    }
    

    public GameObject SpawnAnimatedKanji(Vector3 location, Vector3 rotation, string kanji) {
        var go = new GameObject();

        go.transform.position = location;

        List<LineRenderer> lineRederers = new List<LineRenderer>();
        if (!dict.ContainsKey(kanji)) {
            Debug.Log("Pepega");
            return null;
        }
        var lines = dict[kanji];
        var scale = 0.002f;
        for (var i = 0; i < lines.Count(); ++i) {
            var ngo = new GameObject();
            ngo.transform.parent = go.transform;
            lineRederers.Add(ngo.AddComponent<LineRenderer>());
            var line = lineRederers[lineRederers.Count - 1];
            line.startWidth = .02f;
            line.endWidth = .02f;
            line.material.SetFloat("_Glossiness", 0.0f);
            line.material.color = UnityEngine.Color.red;
            line.positionCount = lines.GetLine(i).Count();

            for (var j = 0; j < lines.GetLine(i).Count(); ++j) {
                var point = scale*lines.GetLine(i).GetPoint(j);
                var pointPos = go.transform.position;
                pointPos.x += point.x;
                pointPos.y -= point.y;
                line.SetPosition(j, pointPos);
            }
        }

        return go;
    }

}
