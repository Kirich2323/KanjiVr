using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

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

    public class SVGCommand {
        public char command { get; private set; }
        public float[] arguments { get; private set; }

        public SVGCommand(char command, params float[] arguments) {
            this.command = command;
            this.arguments = arguments;
        }

        public static SVGCommand Parse(string SVGpathstring) {
            var cmd = SVGpathstring.Take(1).Single();
            
            string remainingargs = SVGpathstring.Substring(1);
            
            string argSeparators = @"[\s,]|(?=-)";
            var splitArgs = Regex
                .Split(remainingargs, argSeparators)
                .Where(t => !string.IsNullOrEmpty(t));
            float[] floatArgs = splitArgs.Select(arg => float.Parse(arg, CultureInfo.InvariantCulture)).ToArray();
            
            return new SVGCommand(cmd, floatArgs);
        }
    }
}
