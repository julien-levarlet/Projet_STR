using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe abtraite représentant un agent (ennemi ou joueur), gérant les déplacement dans la scene.
/// Les classes héritant de Agent controller doivent surcharger <c>GetInputVertical</c> et <c>GetInputHorizontal</c>.
/// </summary>
[RequireComponent(typeof(CharacterController))] // un CharacterController doit être attaché au game-object dans l'éditeur
public abstract class AgentController : MonoBehaviour
{    
    [SerializeField] private float speed = 500;
    [SerializeField] private float rotSpeed = 300;
    [SerializeField] protected Transform target;
    private CharacterController _cc;
    private const float Gravity = 9.81f;
    
    protected virtual void Start()
    {
        _cc = GetComponent<CharacterController>(); // on récupère le CharacterController
    }

    void Update()
    {
        float currentSpeed = GetInputVertical() * speed; // on récupère les valeurs calculées par la classe fille
        float rotation = GetInputHorizontal() * rotSpeed;
        transform.Rotate(0, rotation * Time.deltaTime,0); // application de la rotation
        Vector3 dir = transform.forward * currentSpeed; 
        dir -= new Vector3(0, Gravity, 0); // prise en compte de la gravité en plus de la translation
        _cc.SimpleMove(dir  * Time.deltaTime); // application du déplacement
    }
    
    /// <summary>
    /// Fonction permettant d'obtenir le mouvement de l'agent sur l'axe vertical, correspondant à la translation de l'agent
    /// Correspond à l'appuie sur les touche haut/bas
    /// Remarque :  <c>GetInputVertical</c> est toujours appelée avant <c>GetInputHorizontal</c>
    /// </summary>
    /// <returns>Valeur comprise dans [-1,1]</returns>
    protected abstract float GetInputVertical();

    /// <summary>
    /// Fonction permettant d'obtenir le mouvement de l'agent sur l'axe horizontal, correspondant à la rotation de l'agent
    /// Correspond à l'appuie sur les touche gauche/droite
    /// Remarque :  <c>GetInputVertical</c> est toujours appelée avant <c>GetInputHorizontal</c>
    /// </summary>
    /// <returns>Valeur comprise dans [-1,1]</returns>
    protected abstract float GetInputHorizontal();

    protected void CheckTarget()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            throw new Exception("Aucune cible indiquée pour l'ennemi");
        }
    }
}
