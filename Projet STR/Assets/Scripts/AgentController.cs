using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;


/// <summary>
/// Classe abtraite représentant un agent (ennemi ou joueur), gérant les déplacement dans la scene.
/// Les classes héritant de Agent controller doivent surcharger <c>GetInputVertical</c> et <c>GetInputHorizontal</c>.
/// </summary>
[RequireComponent(typeof(Rigidbody))] // un rigidbody doit être attaché au game-object dans l'éditeur
[RequireComponent(typeof(BoxCollider))]
public abstract class AgentController : MonoBehaviour
{    
    [SerializeField] private float speed = 500;
    [SerializeField] private float rotSpeed = 300;
    [SerializeField] protected Transform target;
    private Rigidbody _rb;
    private Vector3 _defaultPosition;
    private float _move;
    private float _rotation;
    private const int MaxLife = 3;
    private int _life;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _life = MaxLife;
        _defaultPosition = transform.position;
    }

    void Update()
    {
        _move = GetInputVertical(); // on récupère les valeurs calculées par la classe fille
        _rotation = GetInputHorizontal();
    }

    private void FixedUpdate()
    {
        // on ajuste direction et rotation
        _rb.MoveRotation(_rb.rotation *
                         Quaternion.Euler(new Vector3(0, _rotation, 0) * rotSpeed * Time.fixedDeltaTime));
        Vector3 dir = transform.forward * _move * speed * Time.fixedDeltaTime;
        dir.y = _rb.velocity.y; // on conserve la gravité
        _rb.velocity = dir;
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

    /// <summary>
    /// Associe à l'attribut <c>Target</c>, le transforme de l'objet nommé Player dans la scene
    /// </summary>
    protected void GetTarget()
    {
        target = GameObject.Find("Player").transform;  // get the player in the scene
    }

    /// <summary>
    /// Perte d'un point de vie
    /// </summary>
    public void TakeHit()
    {
        if (_life <= 0) 
        {
            // mort du personnage
            gameObject.SetActive(false);
        }
        _life -= 1;
    }

    /// <summary>
    /// Quand on sort de la zone de jeu, on perd un point de vie et on retourne à la position de départ
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name != "Plane") return;
        TakeHit();
        transform.position = _defaultPosition;
    }
}
