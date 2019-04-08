using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
//using System.Xml.Serialization;

public class DrawAnimatedKanji {
    private string kanjiData = "Assets/Resources/zinniadata/handwriting-ja.xml"; //todo: propper path

    public DrawAnimatedKanji() {
        Debug.Log("Constructor");
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        //xmlDoc.LoadXml(kanjiData); // load the file.
        xmlDoc.Load(kanjiData);
        var kanjis = xmlDoc.GetElementsByTagName("character");


        var dict = new Dictionary<string, KanjiLines>();
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
                            var x = int.Parse(point.Attributes["x"].Value);
                            var y = int.Parse(point.Attributes["y"].Value);
                            //Debug.Log(point.Attributes["x"].Value + "   " + point.Attributes["y"].Value);
                            line.addPoint(x, y);
                        }
                        lines.addLine(line);
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
    

    GameObject SpawnAnimatedKanji(Vector3 location, Vector3 rotation, string kanji) {
        return new GameObject();
    }

}
