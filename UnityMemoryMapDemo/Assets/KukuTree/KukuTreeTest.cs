using UnityEngine;
using System.Collections;

public class KukuTreeTest : MonoBehaviour {

    [Range(0,10000)]
    public int numPoints;
    Vector3[] points;
    public bool realtimeUpdate;
    public bool debugPoints;

    public Transform target;

	// Use this for initialization
	void Start () {
        regeneratePoints();
    }
	
	// Update is called once per frame
	void Update () {

        if (realtimeUpdate) regeneratePoints();
        

        if (debugPoints)
        {
            
            for (int i = 0; i < numPoints; i++)
            {
                Color c = (Vector3.Distance(points[i], target.position) < target.localScale.x)?Color.green:Color.red;
                Debug.DrawLine(points[i], points[i] + Vector3.forward * .05f,c);
            }
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target.position,target.localScale.x);
    }

    void regeneratePoints()
    {
        points = new Vector3[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = Random.insideUnitSphere * 2;
        }
    }
}
