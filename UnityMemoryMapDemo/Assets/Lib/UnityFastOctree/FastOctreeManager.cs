using UnityEngine;
using System.Collections;
using System;
//using System.Linq;
using System.Runtime.InteropServices;

//using UnityEditor;

public class FastOctreeManager : MonoBehaviour {

    public bool showPluginConsole;

    public bool generate;
    public int generateCount = 1000;

    Vector3[] points;
    int[] result;

    public bool debugPoints;

    public Vector3 searchPoint;
    public float searchRadius;
    

    static IntPtr nativeLibraryPtr;

    delegate void createOctree_C();
    delegate void fillOctree_C(IntPtr points, int numPoints);
    delegate int getNeighbours_C(Vector3 pos, float radius, IntPtr resultPoints);
    delegate void clean();
    delegate void openConsole();

   
    // Use this for initialization
    void Start () {
        if (nativeLibraryPtr != IntPtr.Zero) return;

        nativeLibraryPtr = Native.LoadLibrary("UnityFastOctreePlugin");
        if (nativeLibraryPtr == IntPtr.Zero)
        {
            Debug.LogError("Failed to load native library");
        }

        if (showPluginConsole)
        {
            Native.Invoke<openConsole>(nativeLibraryPtr);
        }

        Native.Invoke<createOctree_C>(nativeLibraryPtr);

        points = new Vector3[generateCount];
        for (int i = 0; i < generateCount; i++)
        {
            points[i] = UnityEngine.Random.insideUnitSphere;
        }
        result = new int[generateCount];

    }

    // Update is called once per frame
    void Update () {

        if (generate)
        {
            points = new Vector3[generateCount];
            for (int i = 0; i < generateCount; i++)
            {
                points[i] = UnityEngine.Random.insideUnitSphere;
            }

            result = new int[generateCount];
            

        }

        /*
        if (debugPoints)
        {
            for (int i = 0; i < generateCount; i++)
            {
                Debug.DrawLine(points[i], points[i] + Vector3.forward * .05f);
            }
        }
        */

        GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);  
        if (generate) Native.Invoke<fillOctree_C>(nativeLibraryPtr, handle.AddrOfPinnedObject(), generateCount);
        handle.Free();


        int numNeighbours;
        getNeighbours(searchPoint, searchRadius, out numNeighbours);
        for (int j = 0; j < numNeighbours; j+=500)
        {
            Debug.DrawLine(searchPoint, points[result[j]]);
        }

        /*
        for (int i=0;i<generateCount;i++)
        {
            int numNeighbours;
            
            getNeighbours(points[i], searchRadius, out numNeighbours);


            for (int j = 0; j < numNeighbours; j++)
            {
                //Debug.DrawLine(points[i], points[result[j]]);
            }
            
            
        }
        */

    }

    void getNeighbours(Vector3 pos, float radius, out int resultCount)
    {
        GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
        resultCount = Native.Invoke<int, getNeighbours_C>(nativeLibraryPtr, pos, searchRadius, handle.AddrOfPinnedObject());
        handle.Free();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(searchPoint, searchRadius);
    }



    void OnApplicationQuit()
    {
        
        if (nativeLibraryPtr == IntPtr.Zero) return;

        Native.Invoke<clean>(nativeLibraryPtr);

        Debug.Log(Native.FreeLibrary(nativeLibraryPtr)
                      ? "Native library successfully unloaded."
                      : "Native library could not be unloaded.");
    }
}
