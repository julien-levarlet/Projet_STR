using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe se chargeant de v�rifier le bon d�roulement du jeu :
/// - gestion de la sortie de la map
/// - d�but et fin de partie
/// - mise en place de diff�rentes r�gles
/// </summary>
public class GameManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) // gestion de la sortie de la map
    {
        other.gameObject.transform.position = new Vector3(0, 20, 0);
    }
}