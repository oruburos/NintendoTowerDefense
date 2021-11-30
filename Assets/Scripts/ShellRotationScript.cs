using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellRotationScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Quaternion theRotation = transform.localRotation;
        theRotation.z *= Time.deltaTime * 270;
        transform.localRotation = theRotation;

    }
}
