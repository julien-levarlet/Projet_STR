using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Classe charg�e de g�rer l'affichage � l'�cran, en selectionnant les cameras de la scene
/// </summary>
public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera[] cameras; 
    private int _currentCamIndex;

    void Start() // on active seulement la premi�re camera
    {
        _currentCamIndex = 0;
        cameras[0].gameObject.SetActive(true);
        for (int i=1; i<cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // en cas d'appuie sur tab on passe � la camera suivante
        {
            cameras[_currentCamIndex].gameObject.SetActive(false);
            _currentCamIndex = (_currentCamIndex+1)%cameras.Length;
            cameras[_currentCamIndex].gameObject.SetActive(true);
        }
    }
}
