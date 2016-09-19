using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Runtime.InteropServices;

public class MemoryMapManager : MonoBehaviour{

    public bool showPluginConsole;

    static IntPtr nativeLibraryPtr;

    delegate bool setupMemoryShare_C(string memoryName, int memorySize, bool isServer);
    delegate bool connectMemoryShare_C();
    delegate IntPtr getMemoryData_C();
    delegate void openConsole();
    
    bool isSetup = false;
    bool isConnected = false;

    public static MemoryMapManager instance;

    
    void Awake()
    {
        instance = this;

        if (nativeLibraryPtr != IntPtr.Zero) return;

        nativeLibraryPtr = Native.LoadLibrary("UnityMemoryMapPlugin");
        if (nativeLibraryPtr == IntPtr.Zero)
        {
            Debug.LogError("Failed to load native library");
        }

        if(showPluginConsole)
        {
            Native.Invoke<openConsole>(nativeLibraryPtr);
        }
        
    }

    public void setupMemoryShare(string memoryKey,int memorySize,bool isServer)
    {
       Debug.Log("Setup MemoryShare : " + memoryKey + ", with size " + memorySize + ", isServer : " + isServer);
       isSetup = Native.Invoke<bool, setupMemoryShare_C>(nativeLibraryPtr, memoryKey, memorySize , isServer);
    }

    void Update()
    {
       if (!isSetup) return;

       if(!isConnected)
        {
            isConnected = Native.Invoke<bool,connectMemoryShare_C>(nativeLibraryPtr); // Should return the number 50.
            if (isConnected) Debug.Log("Connected !");
        }

        if (!isConnected) return;
    }


    public static void fillData(object data)
    {
        instance.fillDataInternal(data);
    }

    void fillDataInternal(object data)
    {
        if (!isConnected) return;
        
        
        IntPtr rawDataPtr = Native.Invoke<IntPtr, getMemoryData_C>(nativeLibraryPtr);
        Marshal.PtrToStructure(rawDataPtr, data);

        
    }

    void OnApplicationQuit()
    {
        if (nativeLibraryPtr == IntPtr.Zero) return;

        Debug.Log(Native.FreeLibrary(nativeLibraryPtr)
                      ? "Native library successfully unloaded."
                      : "Native library could not be unloaded.");
        isConnected = false;
        isSetup = false;
    }
}
