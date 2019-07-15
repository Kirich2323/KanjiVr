using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

using static Utils;

public class ZinniaRecognition  {

    private IntPtr recognizer;
    private IntPtr zinniaCharacter;

    public ZinniaRecognition() {
        recognizer = Zinnia.zinnia_recognizer_new();
        zinniaCharacter = Zinnia.zinnia_character_new();
        Zinnia.zinnia_recognizer_open(recognizer, Encoding.ASCII.GetBytes("Assets\\CustomResources\\zinniadata\\handwriting-ja.model\0")); //todo: propper path
    }

    public string Recognize(ref List<List<List<float>>> lines, int margin) {
        Debug.Log("Total lines: " + lines.Count.ToString());
        for (int i = 0; i < lines.Count; ++i) {
            Debug.Log("line " + i.ToString() + "   " + lines[i].Count.ToString());
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

        Zinnia.zinnia_character_clear(zinniaCharacter);
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

        //Zinnia.zinnia_character_set_width(zinniaCharacter, (uint)width);   //breaks recognition
        //Zinnia.zinnia_character_set_height(zinniaCharacter, (uint)height); //breaks recognition

        for (int i = 0; i < lines.Count; ++i) {
            for (int j = 0; j < lines[i].Count; ++j) {
                int x = (int)(lines[i][j][0] - minX + margin); //todo: maybe math.round
                int y = (int)(lines[i][j][1] - minY + margin); //todo: maybe math.round
                //Debug.Log(x.ToString() + "  " + y.ToString());
                Zinnia.zinnia_character_add(zinniaCharacter, (uint)i, x, y);
            }
        }

        int topn = 1;
        var result = Zinnia.zinnia_recognizer_classify(recognizer, zinniaCharacter, (uint)topn);
        string ans = "";
        for (int i = 0; i < topn; ++i) {
            ans += PtrToStringUtf8(Zinnia.zinnia_result_value(result, (uint)i)) + " ";
        }

        Zinnia.zinnia_result_destroy(result);
        return ans;
    }

    ~ZinniaRecognition() {
        Zinnia.zinnia_recognizer_close(recognizer);
        Zinnia.zinnia_recognizer_destroy(recognizer);
        Zinnia.zinnia_character_destroy(zinniaCharacter);
    }
}
