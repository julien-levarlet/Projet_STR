using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private List<Camera> cameras;
    private int currentCamIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentCamIndex = 0;
        cameras[0].gameObject.SetActive(true);
        for (int i=1; i<cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameras[currentCamIndex].gameObject.SetActive(false);
            currentCamIndex = (currentCamIndex+1)%cameras.Count;
            cameras[currentCamIndex].gameObject.SetActive(true);
        }
    }
}
