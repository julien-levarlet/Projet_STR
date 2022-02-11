using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 10;
    [SerializeField] private int rotSpeed = 10;
    private Rigidbody rb;
    private float h;
    private float v;

    // Start is called before the first frame update
    void Start()
    {
        h = 0;
        v = 0;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.rotation * new Vector3(0, 0, v * speed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, h, 0) * rotSpeed * Time.fixedDeltaTime));
    }
}
