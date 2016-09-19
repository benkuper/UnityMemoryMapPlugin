using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(PCLDataReceiver))]
public class OctreeManager : MonoBehaviour {

    PCLDataReceiver receiver;
    PointOctree<PCLPointObject> octree;

    [Range(1,20)]
    public int steps;
    [Range(0,2)]
    public float maxDistance;
    public Color nearColor;
    public Color farColor;
    public Material lineMat;

    public bool drawLines;
    public bool drawMesh;
    public bool gameCamsOnly;

    int numPoints;
    int stepNumPoints;

    Mesh m;
    MeshFilter mf;


    // Use this for initialization
    void Start () {
        receiver = GetComponent<PCLDataReceiver>();


        m = new Mesh();
        mf = GetComponent<MeshFilter>();
        mf.mesh = m;
    }
	
	// Update is called once per frame
	void Update () {

        octree = new PointOctree<PCLPointObject>(1, Vector3.zero,.001f);
        numPoints = receiver.pointObjects.Length;
        stepNumPoints = 0;
        for (int i=0;i < numPoints;i+= steps)
        {
            octree.Add(receiver.pointObjects[i], receiver.pointObjects[i].point.position);
            stepNumPoints++;
        }

        if (drawMesh) updateMesh();
        GetComponent<Renderer>().enabled = drawMesh;

    }

    void updateMesh()
    {
        Vector3[] vList = new Vector3[stepNumPoints];
        Color[] colors = new Color[stepNumPoints];
        List<int> indiceList = new List<int>();

        int index = 0;
        for (int i = 0; i < numPoints; i+=steps)
        {
            if (index >= numPoints) break;

            PCLPointObject p = receiver.pointObjects[i];
            Vector3 pPos = p.point.position;

            vList[index] = pPos;
            colors[index] = p.point.color;

            Ray r = new Ray(pPos, Vector3.forward);
            PCLPointObject[] np = octree.GetNearby(r, maxDistance);

            foreach (PCLPointObject npp in np)
            {
                Vector3 nppPos = npp.point.position;

                if (nppPos.x < pPos.x || Vector3.Distance(nppPos, pPos) > maxDistance) continue;

                indiceList.Add(p.index);
                indiceList.Add(npp.index);
            }

            index++;


        }
        /*
        int[] iList = indiceList.ToArray();

        
        m.vertices = vList;
        m.colors = colors;
        m.SetIndices(iList, MeshTopology.Lines, 0);
        m.RecalculateBounds();
        */
    }

    
    public void OnEnable()
    {
        // register the callback when enabling object
        Camera.onPostRender += postRender;
    }
    public void OnDisable()
    {
        // remove the callback when disabling object
        Camera.onPostRender -= postRender;
    }


    void postRender(Camera cam)
    {
        if (!drawLines) return;
        if (gameCamsOnly && cam.cameraType != CameraType.Game) return;
        
        Matrix4x4 mat = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        GL.PushMatrix();
        GL.MultMatrix(mat);
        lineMat.SetPass(0);
        GL.Begin(GL.LINES);

        int numLines = 0;
        
        for (int i = 0; i < numPoints; i += steps)
        {
            PCLPointObject p = receiver.pointObjects[i];

            Vector3 pPos = p.point.position;
            Ray r = new Ray(pPos, Vector3.forward);
            PCLPointObject[] np = octree.GetNearby(r, maxDistance);

            
             GL.Begin(GL.LINES);
            foreach (PCLPointObject npp in np)
            {
                Vector3 nppPos = npp.point.position;

                if (nppPos.x < pPos.x || Vector3.Distance(nppPos, p.point.position) > maxDistance) continue;

                if (drawLines)
                {
                    float dist = Vector3.Distance(nppPos, pPos);
                    float lerpC = Mathf.Clamp01(dist / maxDistance);

                   
                    //Color c = Color.Lerp(npp.point.color, p.point.color, .5f);
                    GL.Color(Color.Lerp(nearColor, farColor, lerpC));

                    GL.TexCoord(Vector3.zero);
                   // GL.TexCoord(new Vector3(Time.time * texSpeed * targetAlpha, 0, 0));
                    GL.Vertex(nppPos);

                    GL.TexCoord(Vector3.one);
                   // GL.TexCoord(new Vector3(Time.time * texSpeed * targetAlpha + 1, 0, 0));
                    GL.Vertex(pPos);
                  

                    numLines++;
                }
            }
            

        }

        GL.End();
        GL.PopMatrix();

    }
    
}
