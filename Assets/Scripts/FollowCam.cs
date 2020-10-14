using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float cameraZ;

    void Awake()
    {
        cameraZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 destination;

        if (POI != null)
        {
            destination = POI.transform.position;

            if(POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }
        else
        {
            destination = Vector3.zero;
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.z = cameraZ;

        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
}
