using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    GameObject launchPoint;

    // Start is called before the first frame update
    void Start()
    {
        launchPoint = transform.Find("LaunchPoint").gameObject;

        launchPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }
}
