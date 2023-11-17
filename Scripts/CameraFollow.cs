using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public bool followY = true; // Set this to false if you only want to track the x position

   /// <summary>
   /// The Update function calculates the desired position of the camera, optionally only tracking the x
   /// position if followY is false, and smoothly moves the camera towards the target position.
   /// </summary>
    private void Update()
    {
        // Calcula a posição desejada da câmera
        Vector3 targetPosition = target.position + offset;

        // Optionally, only track the x position if followY is false
        if (!followY)
        {
            targetPosition.y = transform.position.y;
        }

        // Suaviza o movimento da câmera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
