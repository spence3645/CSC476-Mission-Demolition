using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    [Header("Set in Inspector")]
    public GameObject projectilePrefab;
    public float velocity = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public GameObject projectile;
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

            if (Input.GetMouseButtonUp(0))
            {
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
