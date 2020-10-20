using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    [Header("Set in Inspector")]
    public GameObject projectilePrefab;
    public float velocity = 8f;
    public float minDist = 0.1f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public GameObject projectile;
    public LineRenderer bandLeft;
    public LineRenderer bandRight;
    public Rigidbody projectileRigid;
    public Vector3 launchPos;
    public bool b_aiming;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if(S == null)
            {
                return Vector3.zero;
            }
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        launchPoint = transform.Find("LaunchPoint").gameObject;
        launchPos = launchPoint.transform.position;

        bandLeft = transform.Find("BandLeft").GetComponent<LineRenderer>();
        bandRight = transform.Find("BandRight").GetComponent<LineRenderer>();

        launchPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
    }

    void Aiming()
    {
        if (b_aiming)
        {
            Vector3 mousePos = Input.mousePosition;

            mousePos.z = -Camera.main.transform.position.z;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 mouseDelta = worldPos - launchPos;

            float maxMagnitude = this.GetComponent<SphereCollider>().radius;

            if (mouseDelta.magnitude > maxMagnitude)
            {
                mouseDelta.Normalize();
                mouseDelta *= maxMagnitude;
            }

            Vector3 projPos = launchPos + mouseDelta;

            projectile.transform.position = projPos;

            Vector3 bandPos = new Vector3(mouseDelta.x - 0.5f, mouseDelta.y - 0.7f, mouseDelta.z);

            bandLeft.SetPosition(0, bandPos);
            bandRight.SetPosition(1, bandPos);

            if (Input.GetMouseButtonUp(0))
            {
                bandLeft.SetPosition(0, new Vector3(0, 0, 0));
                bandRight.SetPosition(1, new Vector3(0,0,0));

                b_aiming = false;

                projectileRigid.isKinematic = false;
                projectileRigid.velocity = -mouseDelta * velocity;

                FollowCam.POI = projectile;

                projectile = null;

                MissionDemolition.ShotFired();

                ProjectileLine.S.poi = projectile;
            }
        }
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        b_aiming = true;

        projectile = Instantiate(projectilePrefab, launchPos, Quaternion.identity, null);

        projectileRigid = projectile.GetComponent<Rigidbody>();

        projectileRigid.isKinematic = true;
    }
}
