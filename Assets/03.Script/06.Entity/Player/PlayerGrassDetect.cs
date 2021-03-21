using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerGrassDetect : MonoBehaviour
{
    public Material _SolidGrassMat;
    public Material _TransparentGrassMat;

    private void OnDisable()
    {
        GetComponent<BoxCollider>().enabled = false;

    }

    private void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = true;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            other.GetComponent<Renderer>().material = _TransparentGrassMat;
        }

        if (other.CompareTag("Enemy"))
        {
            print("디텍트!!");
            other.GetComponent<PlayerStats>().isDectet = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            other.GetComponent<MeshRenderer>().material = _SolidGrassMat;
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<PlayerStats>().isDectet = false;
        }
    }
}
