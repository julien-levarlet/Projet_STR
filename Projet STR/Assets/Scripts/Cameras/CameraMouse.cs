using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Camera en vue du dessus dont l'angl est controllée par les mouvements de la souris
/// </summary>
public class CameraMouse : MonoBehaviour
{
    [SerializeField] private float speedH = 4.0f;
    [SerializeField] private float speedV = 4.0f;

    [SerializeField] private float pitch = 20f;
    [SerializeField] private float yaw = 0f;

    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
