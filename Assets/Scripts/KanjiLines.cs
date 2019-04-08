using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiLines {

    public KanjiLines() {
        lines = new List<KanjiLine>();
    }

    public void addLine(KanjiLine line) {
        lines.Add(line);
    }

    private List<KanjiLine> lines; 
}
