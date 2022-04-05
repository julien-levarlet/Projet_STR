using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NEAT;
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
/// Cette est un singleton, il ne peut exister qu'une seule fois dans la scene
/// - gestion de la sortie de la map
/// - début et fin de partie
/// - mise en place de différentes règles
/// </summary>
[RequireComponent(typeof(Collider))]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // instance du singleton
    private GameState _state;
    private GameObject[] _enemies;
    public static GameObject _victory;
    public static GameObject _player;
    public GameObject PrefabVictory;
    private Vector3[] _validPositions;


    public static event Action<GameState> OnGameStateChanged;

    void Awake()  // à l'instanciation
    {
        if (_instance == null) // si on est la première instance on l'affecte à _instance
        {
            _instance = this;
            return;
        }
        // si une instance est déjà existante, cet objet n'est pas valide, il faut le détruire
        Destroy(gameObject);
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        // on récupère les joueurs et les points de spawn
        _player = GameObject.Find("Player");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var positions = GameObject.FindGameObjectsWithTag("Spawn");
        
        _validPositions = new Vector3[positions.Length];
        
        for (int i=0; i<positions.Length; ++i)
            _validPositions[i] = positions[i].transform.position;

        // vérification que toutes les conditions sont réunis pour une partie
        if (_validPositions.Length < _enemies.Length + 2)
        {
            gameObject.SetActive(false);
            Debug.LogError("Le nombre de spawn n'est pas suffisant");
            return;
        }
        if (_enemies.Length < 1)
        {
            gameObject.SetActive(false);
            Debug.LogError("Il n'y a pas d'ennemis dans la scene");
            return;
        }
        if (_player == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("Il n'y a pas de joueur dans la scene");
            return;
        }
        
        Debug.Log("Joueur :" + _player);
        Debug.Log("Nombre d'ennemis : " + _enemies.Length);
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
        for (; index<_enemies.Length; ++index)
            _enemies[index].GetComponent<AgentController>().SetPos(_validPositions[index]);
        
        _player.GetComponent<AgentController>().SetPos(_validPositions[index]);
        
        // placement du point de victoire
        if (_victory == null)
            _victory = Instantiate(PrefabVictory, _validPositions[++index], Quaternion.identity);
        else
            _victory.gameObject.transform.position = _validPositions[++index];
    }
    
    private float distProche = Vector3.Distance(_player.GetComponent<AgentController>().target.position,_victory.gameObject.transform.position);
    private void Update()
    {
        float dist = Vector3.Distance(_player.GetComponent<AgentController>().target.position,
            _victory.gameObject.transform.position);
        if (distProche > dist)
        {
            float ecart = distProche - dist;
            _player.GetComponent<NeatAgent>().Reward(ecart);
            distProche = dist;
        }
        
        //Mise a jour alive
        //Si ennemi meurt passe à false
        //Mise a jour distProche
        //Si jamais distance joueur objectif a diminué donne point au joueur de la distance diminuée * 10 

        // redemarrage de la position des agents
        if (Input.GetKey(KeyCode.P))
            SetPositions();
        // redemarrage de la partie
        if (Input.GetKey(KeyCode.R))
            Restart();
        
        if (!_player.activeSelf)
        {
            UpdateGameState(GameState.Lose);
        }
        //donne des points à l'ennemi en fonctin du temps et plus le joueur est blessé
        _enemies[0].GetComponent<AgentController>().Reward(2);
        _enemies[0].GetComponent<NeatAgent>().Reward(3*(AgentController.MaxLife-_enemies[0].GetComponent<AgentController>()._life));
        
        //donne des points au joueur tant qu'il n'est pas blessé
        if (_player.GetComponent<AgentController>()._life==AgentController.MaxLife)
        {
            _player.GetComponent<NeatAgent>().Reward(3);
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
        //update fitness
        _player.GetComponent<AgentController>().Reward(10000);
        
        //reward joueur avec le temps
       // _playerr.Reward(1000/Temps passé);
       for (int i = 0; i < _enemies.Length; i++)
       {
           if (_enemies[i].GetComponent<AgentController>()._life == 0)
           {
               _player.GetComponent<AgentController>().Reward(50);
           }
       }
        //renvoi fitness au RDN
        Restart();
    }

    public void PlayerLost()
    {
        Debug.Log("Le joueur a perdu");
        //update fitness
        _enemies[0].GetComponent<AgentController>().Reward(10000);
        
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i].GetComponent<AgentController>()._life == 0)
            {
                _player.GetComponent<AgentController>().Reward(50);
            }
        }
        //renvoi fitness au RDN
        Restart();
    }


    /// <summary>
    /// Quand on sort de la zone de jeu, on perd un point de vie et on retourne à la position de départ
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var agent = other.gameObject.GetComponent<AgentController>();
        agent.TakeHit();
        agent.ResetPos();
    }
}