using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Zinnia {

    // Character
    [DllImport("libzinnia.dll")]
    public extern static IntPtr zinnia_version();

    [DllImport("libzinnia.dll")]
    public extern static IntPtr zinnia_character_new();

    [DllImport("libzinnia.dll")]
    public extern static void zinnia_character_destroy(IntPtr character);

    [DllImport("libzinnia.dll")]
    public extern static int zinnia_character_add(IntPtr character, uint id, int x, int y);

    [DllImport("libzinnia.dll")]
    public extern static uint zinnia_character_width(IntPtr character);

    [DllImport("libzinnia.dll")]
    public extern static uint zinnia_character_height(IntPtr character);

    [DllImport("libzinnia.dll")]
    public extern static void zinnia_character_set_width(IntPtr character, uint width);

    [DllImport("libzinnia.dll")]
    public extern static void zinnia_character_set_height(IntPtr character, uint height);

    [DllImport("libzinnia.dll")]
    public extern static void zinnia_character_clear(IntPtr character);


    // Result
    [DllImport("libzinnia.dll")]
    public extern static IntPtr zinnia_result_value(IntPtr result, uint i);

    [DllImport("libzinnia.dll")]
    public extern static void zinnia_result_destroy(IntPtr result);


    //Recognizer
    [DllImport("libzinnia.dll")]
    public extern static IntPtr zinnia_recognizer_new();

    [DllImport("libzinnia.dll")]
    public extern static void zinnia_recognizer_destroy(IntPtr recognizer);

    [DllImport("libzinnia.dll")]
    public extern static int zinnia_recognizer_open(IntPtr recognizer, byte[] filename);

    [DllImport("libzinnia.dll")]
    public extern static int zinnia_recognizer_close(IntPtr recognizer);

    [DllImport("libzinnia.dll")]
    public extern static IntPtr zinnia_recognizer_strerror(IntPtr recognizer);

    [DllImport("libzinnia.dll")]
    public extern static int zinnia_recognizer_size(IntPtr recognizer);

    [DllImport("libzinnia.dll")]
    public extern static IntPtr zinnia_recognizer_classify(IntPtr recognizer, IntPtr character, uint nbest);
}
