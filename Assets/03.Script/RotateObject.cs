using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotateAmount;

    private void Update()
    {
        this.transform.Rotate(rotateAmount * Time.deltaTime, Space.Self);
    }
}
