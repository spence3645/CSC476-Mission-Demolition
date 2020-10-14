using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Projectile")
        {
            Debug.Log("Goal!");
            goalMet = true;

            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;

            c.a = 1;

            mat.color = c;
        }
    }
}
