using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class AgentController : MonoBehaviour
{    
    [SerializeField] private float speed = 500;
    [SerializeField] private float rotSpeed = 300;
    private float gravity = 9.81f;
    private CharacterController _cc;

    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float currentSpeed = GetInputVertical() * speed;
        float rotation = GetInputHorizontal() * rotSpeed;
        transform.Rotate(0, rotation * Time.deltaTime,0); // application de la rotation
        Vector3 dir = transform.forward * currentSpeed;
        dir -= new Vector3(0, gravity, 0); // prise en compte de la gravité
        _cc.SimpleMove(dir  * Time.deltaTime); // application du déplacement
    }

    protected abstract float GetInputVertical();

    protected abstract float GetInputHorizontal();
}
