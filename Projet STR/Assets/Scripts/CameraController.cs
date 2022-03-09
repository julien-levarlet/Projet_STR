using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mouvements de camera en vue du dessus en utilisant les mouvements de la souris et les touches wasd
/// </summary>
public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit = new Vector2(30,30);
    public float minY = 0f;
    public float maxY = 100f;
    public float scrollSpeed = 20f;

    void Update()
    {
        Vector3 pos=transform.position;

        if (Input.GetKey("w")|Input.mousePosition.y>=Screen.height-panBorderThickness)
        {
            pos.z+=panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") | Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") | Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") | Input.mousePosition.x <=panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll= Input.GetAxis("Mouse ScrollWheel");
        pos.y+=scroll * scrollSpeed * 50f * Time.deltaTime;
        //On a des mvmts de cam�ra tres libre, on peut restreindre pour que le joueur ne voit que la map actuelle/ On pourrait restreindre en fonction de sa position
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        pos.y = Mathf.Clamp(pos.y, minY, maxY); 
        transform.position=pos;
    }
}
