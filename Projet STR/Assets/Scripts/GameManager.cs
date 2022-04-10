using System;
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
    private float initDistEnemy; // distance initiale entre l'ennemi et le joueur
    private float initDistEnemyObj;
    private float rewTime=0;
    private int life = AgentController.MaxLife;
    [SerializeField] private bool isTraining;
    [SerializeField] private float _beginTime;

    public static event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        // on récupère les joueurs et les points de spawn
        //player = GameObject.Find("Player");
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //var positions = GameObject.FindGameObjectsWithTag("Spawn");
        initDistEnemy  = Vector3.Distance(enemies[0].transform.position,player.transform.position);
        initDistPlayer = Vector3.Distance(player.transform.position,victory.gameObject.transform.position);
        initDistEnemyObj = Vector3.Distance(enemies[0].transform.position,victory.gameObject.transform.position);

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
        _beginTime = Time.time;
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
            en.GetComponent<AgentController>().life = AgentController.MaxLife;
        }
        
        /*var rnd = new Random();
        for (int i = 0;i < _validPositions.Length;++i)
        {
            var randomIndex = rnd.Next(_validPositions.Length);
            (_validPositions[randomIndex], _validPositions[i]) = (_validPositions[i], _validPositions[randomIndex]);
        }*/

        int index=0;
        player.GetComponent<AgentController>().SetPos(_validPositions[index]);
        index++;
        for (; index < enemies.Length+1; ++index)
        {   
            //Debug.Log(enemies.Length);
            enemies[index-1].GetComponent<AgentController>().SetPos(_validPositions[index]);
        }

        //player.GetComponent<AgentController>().SetPos(_validPositions[index]);
        
        // placement du point de victoire
        victory.transform.position = _validPositions[index];
    }
    
    private void Update()
    {
        if (isTraining) // En cas d'entrainement
        {
            float dist = Vector3.Distance(player.transform.position,
                victory.gameObject.transform.position);
            if (initDistPlayer > dist)
            {   
                player.GetComponent<NeatAgent>().Reward((initDistPlayer+ - dist)*10);
                rewTime += (initDistPlayer + -dist) * 10;
                //initDistPlayer = dist;
            }

            dist = Vector3.Distance(enemies[0].transform.position, player.transform.position);
            if (initDistEnemy > dist)
            {
                enemies[0].GetComponent<NeatAgent>().Reward((initDistEnemy - dist)*10);
            }
            
            dist = Vector3.Distance(enemies[0].transform.position, victory.gameObject.transform.position);
            if (initDistEnemyObj> dist)
            {
                enemies[0].GetComponent<NeatAgent>().Reward((initDistEnemyObj - dist)*3);
            }
            
            //donne des points à l'ennemi  plus le joueur est blessé
            if (player.GetComponent<NeatAgent>().life < life)
            {
                life = player.GetComponent<NeatAgent>().life;
                enemies[0].GetComponent<NeatAgent>().Reward(20000);
            }
            //donne des points à l'ennemi si il est en vie :
            if (enemies[0].GetComponent<NeatAgent>().life > 0)
            {
                enemies[0].GetComponent<NeatAgent>().Reward(2);
            }
            //enemies[0].GetComponent<NeatAgent>().Reward(10*(AgentController.MaxLife-player.GetComponent<AgentController>().life));
        
            //donne des points au joueur tant qu'il n'est pas blessé
            //if (player.GetComponent<AgentController>().life==AgentController.MaxLife)
            {
                //player.GetComponent<NeatAgent>().Reward(3);
            }
        }
        else
        {
            // contrainte de brieveté
            for (int i = 0; i < enemies.Length; ++i)
            {
                if (Vector3.Distance(player.transform.position, enemies[i].transform.position) < 10)
                {
                    TestsManager.GetInstance().BeginDetection();
                }
            }

            // redemarrage de la position des agents
            if (Input.GetKey(KeyCode.P))
                SetPositions();
            // redemarrage de la partie
            if (Input.GetKey(KeyCode.R))
                Restart();
        }

        // verifier si le joueur est mort
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
        Debug.Log("Le joueur a gagné");
       
        if (!isTraining)
            
            Restart();
        else
        {
            //update fitness
            player.GetComponent<NeatAgent>().Reward(100000);

            //reward joueur avec le temps
            player.GetComponent<NeatAgent>().Reward(rewTime*2);
            print(Time.time-_beginTime);
            // playerr.Reward(1000/Temps passé);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].GetComponent<AgentController>().life == 0)
                {
                    player.GetComponent<NeatAgent>().Reward(50);
                }
            }
            player.SetActive(false);
            enemies[0].SetActive(false);
            UpdateGameState(GameState.StageCleared);
        }
    }

    public void PlayerLost()
    {
        Debug.Log("Le joueur a perdu");
        //update fitness
        
        if (!isTraining)
            Restart();
        else
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].GetComponent<AgentController>().life <= 0)
                {
                    player.GetComponent<NeatAgent>().Reward(50);
                }
            }
            player.SetActive(false);
            enemies[0].SetActive(false);
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