using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NEAT;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
    private float initDistPlayer; // distance initiale entre le joueur et l'objectif
    private float initDistEnemy; // distance 
    [SerializeField] private bool isTraining;

    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        // on récupère les joueurs et les points de spawn
        //player = GameObject.Find("Player");
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //var positions = GameObject.FindGameObjectsWithTag("Spawn");
        initDistPlayer = Vector3.Distance(player.transform.position,victory.gameObject.transform.position);

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

        if (!isTraining)
        {
            Debug.Log("Joueur :" + player);
            Debug.Log("Nombre d'ennemis : " + enemies.Length);
            Debug.Log("Nombre de point de spawn : " + _validPositions.Length);
        }

        SetPositions();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetPositions()
    {
        // On mélange la liste des position pour en affecter une alétoire pour chaque agent
        // https://stackoverflow.com/questions/14473321/generating-random-unique-values-c-sharp
        
        player.SetActive(true);
        player.GetComponent<AgentController>().life = AgentController.MaxLife;

        foreach (var en in enemies)
        {
            en.SetActive(true);
            en.GetComponent<AgentController>().life = 1;
        }
        
        var rnd = new Random();
        for (int i = 0;i < _validPositions.Length;++i)
        {
            var randomIndex = rnd.Next(_validPositions.Length);
            (_validPositions[randomIndex], _validPositions[i]) = (_validPositions[i], _validPositions[randomIndex]);
        }

        int index=0;
        for (; index < enemies.Length; ++index)
        {
            enemies[index].GetComponent<AgentController>().SetPos(_validPositions[index]);
        }

        player.GetComponent<AgentController>().SetPos(_validPositions[index]);
        
        // placement du point de victoire
        victory.transform.position = _validPositions[++index];
    }
    
    private void Update()
    {
        if (isTraining) // En cas d'entrainement
        {
            float dist = Vector3.Distance(player.transform.position,
                victory.gameObject.transform.position);
            if (initDistPlayer > dist)
            {
                player.GetComponent<NeatAgent>().Reward((initDistPlayer - dist)*10);
                //initDistPlayer = dist;
            }

            dist = Vector3.Distance(enemies[0].transform.position, player.transform.position);
            if (initDistEnemy > dist)
            {
                enemies[0].GetComponent<NeatAgent>().Reward((initDistEnemy - dist)*10);
                //initDistPlayer = dist;
            }
            
            //donne des points à l'ennemi en fonction du temps et plus le joueur est blessé
            enemies[0].GetComponent<NeatAgent>().Reward(2);
            enemies[0].GetComponent<NeatAgent>().Reward(3*(AgentController.MaxLife-enemies[0].GetComponent<AgentController>().life));
        
            //donne des points au joueur tant qu'il n'est pas blessé
            if (player.GetComponent<AgentController>().life==AgentController.MaxLife)
            {
                //player.GetComponent<NeatAgent>().Reward(3);
            }
        }

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
        if (_state == state)
            return;
        
        _state = state;

        switch(state)
        {
            case GameState.GameStart:
                break;
            case GameState.InProgress:
                break;
            case GameState.StageCleared:
                NotifyLearningManager();
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
        //Debug.Log("Le joueur a gagné");
        //update fitness
        player.GetComponent<NeatAgent>().Reward(10000);
        
        //reward joueur avec le temps
       // playerr.Reward(1000/Temps passé);
       for (int i = 0; i < enemies.Length; i++)
       {
           if (enemies[i].GetComponent<AgentController>().life == 0)
           {
               player.GetComponent<NeatAgent>().Reward(50);
           }
       }
       
        if (!isTraining)
            Restart();
        else
        {
            UpdateGameState(GameState.StageCleared);
        }
    }

    public void PlayerLost()
    {
        //Debug.Log("Le joueur a perdu");
        //update fitness
        enemies[0].GetComponent<NeatAgent>().Reward(10000);
        
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<AgentController>().life <= 0)
            {
                player.GetComponent<NeatAgent>().Reward(50);
            }
        }
        
        if (!isTraining)
            Restart();
        else
        {
            UpdateGameState(GameState.StageCleared);
        }
    }

    private void NotifyLearningManager()
    {
        LearningManager.GetInstance().GameFinished();
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