using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiLines {

    public KanjiLines() {
        lines = new List<KanjiLine>();
    }

    public void AddLine(KanjiLine line) {
        lines.Add(line);
    }

    public int Count() { return lines.Count; }

    public KanjiLine GetLine(int idx) { return lines[idx]; }

    private List<KanjiLine> lines; 
}
