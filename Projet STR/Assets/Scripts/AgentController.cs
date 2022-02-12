using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentController : MonoBehaviour
{    
    [SerializeField] private float speed = 500;
    [SerializeField] private float rotSpeed = 300;
    private Rigidbody rb;
    private Vector3 deltaPosition;
    private Vector3 deltaRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        deltaPosition = new Vector3(0,0,0);
        deltaRotation = new Vector3(0,0,0);
    }

    void Update()
    {
        deltaPosition.Set(0, 0, GetDeltaPosition());
        deltaRotation.Set(0, GetDeltaRotation(), 0);
    }

    void FixedUpdate()
    {
        rb.velocity = rb.rotation * (deltaPosition * speed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(deltaRotation * rotSpeed * Time.fixedDeltaTime));
    }

    protected abstract float GetDeltaPosition();

    protected abstract float GetDeltaRotation();
}
