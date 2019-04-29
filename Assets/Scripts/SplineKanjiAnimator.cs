using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SplineKanjiAnimator {
    private string kanjiDataPath = "Assets/Resources/kanjivgdata/kanjivg.xml"; //todo: propper path
    public SplineKanjiAnimator() {
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.Load(kanjiDataPath);
        var kanjiData = xmlDoc.GetElementsByTagName("kanji");
        Debug.Log(kanjiData.Count);
        foreach (XmlNode kanjiXml in kanjiData) {
            var graph = kanjiXml.FirstChild;
            foreach(XmlNode radicalNode in graph) {
                if (radicalNode.Name != "g") continue;
                var element = radicalNode.Attributes["kvg:element"];
                if (element == null) {

                }
                else {
                    Radical radical = new Radical(element.Value);
                }
            }
        }
    }

    private Radical FindRadicalInXmlNode(XmlNode node) {
        return new Radical();
    }
}
