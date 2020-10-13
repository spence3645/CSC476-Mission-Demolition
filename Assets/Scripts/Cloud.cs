using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject cloudSphere;
    public int numSpheresMin = 6;
    public int numSpheresMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;

    private List<GameObject> spheres;

    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSpheresMin, numSpheresMax);

        for(int i = 0; i < num; i++)
        {
            GameObject cloud = Instantiate<GameObject>(cloudSphere);
            spheres.Add(cloud);
            Transform cloudTrans = cloud.transform;
            cloudTrans.SetParent(this.transform);
            Vector3 offset = Random.insideUnitSphere;

            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;

            cloudTrans.localPosition = offset;
            Vector3 scale = Vector3.one;

            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeY.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x)/sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            cloudTrans.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            //Restart();
        //}
    }

    void Restart()
    {
        foreach(GameObject cloud in spheres)
        {
            Destroy(cloud);
        }

        Start();
    }
}
