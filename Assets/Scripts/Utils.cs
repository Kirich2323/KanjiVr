using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils {

    public static string PtrToStringUtf8(IntPtr ptr) { // aPtr is nul-terminated
        if (ptr == IntPtr.Zero)
            return "";
        int len = 0;
        while (System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0)
            len++;
        if (len == 0)
            return "";
        byte[] array = new byte[len];
        System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
        return System.Text.Encoding.UTF8.GetString(array);
    }
}
