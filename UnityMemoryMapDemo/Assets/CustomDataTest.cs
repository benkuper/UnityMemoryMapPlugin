using UnityEngine;
using System.Collections;

public class CustomDataTest : MonoBehaviour {

    public string memoryKey;
    public bool isServer;

    [Range(1,20)]
    public int step = 4;

    CustomData data;

	// Use this for initialization
	void Start () {
        data = new CustomData();
        int size = System.Runtime.InteropServices.Marshal.SizeOf(data);
        MemoryMapManager.instance.setupMemoryShare(memoryKey, size, isServer);
    }
	
	// Update is called once per frame
	void Update () {
         MemoryMapManager.fillData(data);
        if (data.message == null) return;

        int dLen = data.message.Length;

        float lineW = 300;
        for(int i=0;i< dLen;i+=step)
        {
            float tx = (i % lineW) * 10 / lineW;
            float ty = Mathf.Floor(i / lineW) * 10 / lineW;
            Debug.DrawLine(new Vector3(tx, ty, data.message[i]), new Vector3(tx, ty, data.message[i] + .05f*data.mouseX/100),new Color32(255,255,255,50));
        }
	}

}
