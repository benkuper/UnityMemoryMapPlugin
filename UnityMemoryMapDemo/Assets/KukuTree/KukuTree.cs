using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KukuTree {

    public Vector3[] points;
    //public KukuNeighbours[] neighArray;
    public List<KukuLink> links;

    public KukuTree()
    {
        
    }

    public void generateNeighbours(Vector3[] sourcePoints, float distance)
    {

        points = sourcePoints;
        Array.Sort(points, delegate (Vector3 p1, Vector3 p2) {
            return p1.x.CompareTo(p2.x);
        });

        //neighArray = new KukuNeighbours[points.Length];

        links = new List<KukuLink>();

        for (int i=0;i<points.Length-1; i++)
        {

            int j = i+1;
            Vector3 pt = points[i];

            //if(neighArray[i] == null) neighArray[i] = new KukuNeighbours();

            while (points[j].x - pt.x <distance){
                if(Vector3.Distance(pt, points[j])< distance)
                {

                    //neighArray[i].addNeigh(j);
                    links.Add(new KukuLink(points[i], points[j]));

                    // if (neighArray[j] == null) neighArray[j] = new KukuNeighbours();
                    // neighArray[j].addNeigh(i);
                }
                j++;
            }
        }
    }

    

}

public class KukuLink
{
    Vector3 start;
    Vector3 end;
    public KukuLink(Vector3 _start, Vector3 _end)
    {
        start = _start;
        end = _end;
    }
}

/*
public class KukuNeighbours
{
    List<int> neighList;

    public KukuNeighbours()
    {
        neighList = new List<int>();
    }

    public void addNeigh(int val)
    {
        neighList.Add(val);
    }
}*/