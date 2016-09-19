using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Vec3
{
    public float x;
    public float y;
    public float z;
};


[StructLayout(LayoutKind.Sequential)]
public struct PCLPoint
{
    public Vector3 position;
    public Color color;
};


[StructLayout(LayoutKind.Sequential)]
public class PCLData {
    [MarshalAs(UnmanagedType.ByValArray,ArraySubType = UnmanagedType.Struct, SizeConst = 1000)]
    public PCLPoint[] points;
};


//After Marshalling, for class use (templates...)
public class PCLPointObject
{
    public PCLPoint point;
    public int index;
    public PCLPointObject(PCLPoint p, int index)
    {
        point = p;
        this.index = index;
    }
}



