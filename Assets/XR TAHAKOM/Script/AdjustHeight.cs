using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class AdjustHeight : MonoBehaviour
{
    public Transform userCamera; // Reference to the VR camera
    public float heightAdjustment = 0.0f; // Amount to adjust the height by

    void Start()
    {
        if (userCamera == null)
        {
            userCamera = Camera.main.transform; // Default to the main camera if none is specified
        }
    }

    void Update()
    {
        AdjustUserHeight();
    }

    void AdjustUserHeight()
    {
        Vector3 adjustedPosition = userCamera.localPosition;
        adjustedPosition.y += heightAdjustment;
        userCamera.localPosition = adjustedPosition;
    }
}
