using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private Vector3 distance = new Vector3(0, 0, -5);
    //private float acceleration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = playerTransform.rotation;
        transform.position = playerTransform.position + distance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerTransform.rotation;
        transform.position = playerTransform.position + transform.rotation * distance;
    }
}
