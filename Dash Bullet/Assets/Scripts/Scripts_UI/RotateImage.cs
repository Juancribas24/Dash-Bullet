using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 1f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto sobre el eje Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

    }
}
