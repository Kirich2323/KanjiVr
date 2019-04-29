using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kanji {
    public Kanji(string KanjiString, List<Radical> KanjiRadicals) {
        Radicals = KanjiRadicals;
        Name = KanjiString;
    }

    public Kanji(string KanjiString) {
        Radicals = new List<Radical>();
        Name = KanjiString;
    }

    public void AddRadical(Radical NewRadical) {
        Radicals.Add(NewRadical);
    }

    private List<Radical> Radicals;
    public string Name {
        get; private set;
    }
}
