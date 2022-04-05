using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public enum GameState
{
    GameStart,
    InProgress,
    StageCleared,
    Victory,
    Lose
}

/// <summary>
/// Classe se chargeant de vérifier le bon déroulement du jeu :
/// - gestion de la sortie de la map
/// - début et fin de partie
/// - mise en place de différentes règles
/// </summary>
[RequireComponent(typeof(Collider))]
public class GameManager : MonoBehaviour
{
    private GameState _state;
    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject victory;
    private Vector3[] _validPositions;

    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        // on récupère les joueurs et les points de spawn
        //player = GameObject.Find("Player");
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //var positions = GameObject.FindGameObjectsWithTag("Spawn");
        
        _validPositions = new Vector3[spawns.Length];
        
        for (int i=0; i<spawns.Length; ++i)
            _validPositions[i] = spawns[i].transform.position;

        // vérification que toutes les conditions sont réunis pour une partie
        if (_validPositions.Length < enemies.Length + 2)
        {
            gameObject.SetActive(false);
            Debug.LogError("Le nombre de spawn n'est pas suffisant");
            return;
        }
        if (enemies.Length < 1)
        {
            gameObject.SetActive(false);
            Debug.LogError("Il n'y a pas d'ennemis dans la scene");
            return;
        }
        if (player == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("Il n'y a pas de joueur dans la scene");
            return;
        }
        
        Debug.Log("Joueur :" + player);
        Debug.Log("Nombre d'ennemis : " + enemies.Length);
        Debug.Log("Nombre de point de spawn : " + _validPositions.Length);
        
        SetPositions();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetPositions()
    {
        // On mélange la liste des position pour en affecter une alétoire pour chaque agent
        // https://stackoverflow.com/questions/14473321/generating-random-unique-values-c-sharp
        var rnd = new Random();
        for (int i = 0;i < _validPositions.Length;++i)
        {
            var randomIndex = rnd.Next(_validPositions.Length);
            (_validPositions[randomIndex], _validPositions[i]) = (_validPositions[i], _validPositions[randomIndex]);
        }

        int index=0;
        for (; index<enemies.Length; ++index)
            enemies[index].GetComponent<AgentController>().SetPos(_validPositions[index]);
        
        player.GetComponent<AgentController>().SetPos(_validPositions[index]);
        
        // placement du point de victoire
        victory.transform.position = _validPositions[++index];
    }

    private void Update()
    {
        // redemarrage de la position des agents
        if (Input.GetKey(KeyCode.P))
            SetPositions();
        // redemarrage de la partie
        if (Input.GetKey(KeyCode.R))
            Restart();
        
        if (!player.activeSelf)
        {
            UpdateGameState(GameState.Lose);
        }
    }

    public void UpdateGameState(GameState state)
    {
        _state = state;

        switch(state)
        {
            case GameState.GameStart:
                break;
            case GameState.InProgress:
                break;
            case GameState.StageCleared:
                break;
            case GameState.Victory:
                PlayerWon();
                break;
            case GameState.Lose:
                PlayerLost();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        OnGameStateChanged?.Invoke(state);
    }

    public void PlayerWon()
    {
        Debug.Log("Le joueur a gagné");
        Restart();
    }

    public void PlayerLost()
    {
        Debug.Log("Le joueur a perdu");
        Restart();
    }


    /// <summary>
    /// Quand on sort de la zone de jeu, on perd un point de vie et on retourne à la position de départ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        var agent = other.gameObject.GetComponent<AgentController>();
        if (agent != null)
        {
            agent.TakeHit();
            agent.ResetPos();
        }
    }
}