using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    public LineRenderer linePrefab;

    //private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    public List<LineRenderer> lines;
    public int numOfLines = 3;
    private int currentIndex = 0;

    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;

            if(_poi != null)
            {
                NewLine();

                //line.enabled = false;
                points = new List<Vector3>();
                AddPointTest(lines[currentIndex]);
            }
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                return (Vector3.zero);
            }

            return (points[points.Count - 1]);
        }
    }

    void Awake()
    {
        S = this;

        //line = GetComponent<LineRenderer>();

        //line.enabled = false;

        lines = new List<LineRenderer>();

        points = new List<Vector3>();
    }

    void FixedUpdate()
    {
        if(poi == null)
        {
            if(FollowCam.POI != null)
            {
                if(FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        AddPointTest(lines[currentIndex]);

        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }

    public void Clear()
    {
        _poi = null;
        //line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;

        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }

        if(points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            points.Add(pt + launchPosDiff);
            points.Add(pt);

            //line.positionCount = 2;
            //line.SetPosition(0, points[0]);
            //line.SetPosition(1, points[1]);
            //line.enabled = true;
        }
        else
        {
            points.Add(pt);
            //line.positionCount = points.Count;
            //line.SetPosition(points.Count - 1, lastPoint);
            //line.enabled = true;
        }
    }


    void NewLine()
    {
        LineRenderer l = Instantiate<LineRenderer>(linePrefab);

        if(lines.Count == 0)
        {
            lines.Insert(currentIndex, l);
            currentIndex = lines.Count - 1;
            return;
        }

        if(lines.Count < numOfLines)
        {
            currentIndex = lines.Count - 1;
            lines.Insert(currentIndex, l);
        }
        else
        {
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
            lines.Insert(0, l);
        }
    }

    public void AddPointTest(LineRenderer l)
    {
        Vector3 pt = _poi.transform.position;

        Vector3 lastPos = l.GetPosition(l.positionCount-1);

        Debug.Log(pt);

        if (l.positionCount > 0 && (pt - lastPos).magnitude < minDist)
        {
            return;
        }

        if (l.positionCount == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            l.positionCount = 2;
            l.SetPosition(0, pt+launchPosDiff);
            l.SetPosition(1, pt);
        }
        else
        {
            l.positionCount++;
            l.SetPosition(l.positionCount-1, pt);
        }
    }
}
