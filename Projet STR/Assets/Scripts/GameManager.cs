using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe se chargeant de vérifier le bon déroulement du jeu :
/// - gestion de la sortie de la map
/// - début et fin de partie
/// - mise en place de différentes règles
/// </summary>
public class GameManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) // gestion de la sortie de la map
    {
        other.gameObject.transform.position = new Vector3(0, 20, 0);
    }
}