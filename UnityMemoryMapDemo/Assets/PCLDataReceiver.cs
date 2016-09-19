using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCLDataReceiver : MonoBehaviour
{

    public PCLData data { get; private set; }

    [Range(1,20)]
    public int steps = 4;
    public Material lineMat;

    public bool drawPoints;
    public bool gameCamsOnly;

    
    Mesh m;
    MeshFilter mf;

    public PCLPointObject[] pointObjects;

    // Use this for initialization
    void Start() {
        data = new PCLData();
        int size = System.Runtime.InteropServices.Marshal.SizeOf(data);
        MemoryMapManager.instance.setupMemoryShare("ofxSharedMemory", size, false);

        /*
        m = new Mesh();
        mf = GetComponent<MeshFilter>();
        mf.mesh = m;
        */
       
    }
	
	// Update is called once per frame
	void Update () {
        MemoryMapManager.fillData(data);
        if (data.points == null) return;

        
        int numPoints = data.points.Length;
        int stepNumPoints = numPoints/steps;

        /*
        Vector3[] vList = new Vector3[stepNumPoints];
        int[] iList = new int[stepNumPoints];
        Color[] colors = new Color[stepNumPoints];
        */

        pointObjects = new PCLPointObject[stepNumPoints];

        for (int i=0;i< stepNumPoints; i++)
        {

            int index = i * steps;
            if (index >= numPoints) break;
            /*
            vList[i] = data.points[index].position;
            iList[i] = i;
            colors[i] = data.points[index].color;
            */
            pointObjects[i] = new PCLPointObject(data.points[index],i);
        }

        /*
        m.vertices = vList;
        m.colors = colors;
        m.SetIndices(iList, MeshTopology.Points, 0);
        m.RecalculateBounds();
        */
        
    }

    public void OnEnable()
    {
        // register the callback when enabling object
        //Camera.onPostRender += postRender;
    }
    public void OnDisable()
    {
        // remove the callback when disabling object
        //Camera.onPostRender -= postRender;
    }

    void postRender(Camera cam)
    {
        if (!drawPoints) return;
        if (gameCamsOnly && cam.cameraType != CameraType.Game) return;

        
        Matrix4x4 mat = Matrix4x4.TRS(transform.position,transform.rotation,transform.lossyScale);

        GL.PushMatrix();
        GL.MultMatrix(mat);
        lineMat.SetPass(0);
        GL.Begin(GL.LINES);

        for (int i = 0; i < data.points.Length; i++)
        {
            if (i % steps != 0) continue;

            PCLPoint p = data.points[i];
            
            GL.Color(p.color);
           // GL.TexCoord(Vector3.zero);
            GL.Vertex(p.position);

           // GL.Color(p.color);
           // GL.TexCoord(Vector3.one);
            GL.Vertex(p.position+Vector3.forward*.01f);

            

            //Debug.DrawLine(p.position, p.position + Vector3.forward * .05f, p.color);
        }

        GL.End();
        GL.PopMatrix();
    }

}

