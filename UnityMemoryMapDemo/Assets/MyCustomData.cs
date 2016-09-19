using System;
using UnityEngine;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public class CustomData {
    public float time;
    public int mouseX;
    public int mouseY;
    
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100000)]
    public float[] message;
}
/*
    public CustomData()
    {
        Debug.Log(Marshal.SizeOf(this));
    }
    
}
*/
