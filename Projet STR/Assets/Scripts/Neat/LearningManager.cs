using System;
using System.Collections.Generic;
using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Regroupement de la gestion de la population de l'algorithme neat, avec les objets de Unity
    /// Implémente le design pattern singletton
    /// </summary>
    public class LearningManager: MonoBehaviour
    {
        private const int Inputs = 7; // notre angle, notre position, position du joueur et position de l'objectif
        private const int Outputs = 3;
        public int populationSize = 200;

        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private int gameDuration = 30; // temps d'une génération en secondes
        private NeatAgent[] _players;
        private NeatAgent[] _enemies;
        private Population _playerPopulation;
        private Population _enemyPopulation;
        private GameManager[] _gameManagers;
        private int _gameFinished;
        private float _beginTime;

        private static LearningManager _instance;

        private void Awake()
        {
            if (_instance==null)
            {
                _instance = this;
            }
            else
            {
                Debug.LogError("Plusieurs instantiations de Learning Manager interdites");
            }
        }

        public static LearningManager GetInstance()
        {
            return _instance;
        }

        private void Start()
        {
            // Création des singletons
            Crossover.Initialise();
            Mutation.Initialise();
            NetworkFactory.Initialise();

            _playerPopulation = new Population();
            _enemyPopulation = new Population();

            _players = new NeatAgent[populationSize];
            _enemies = new NeatAgent[populationSize];
            
            // mise en place de la population
            _playerPopulation.GenerateBasePopulation(populationSize, Inputs, Outputs);
            _enemyPopulation.GenerateBasePopulation(populationSize, Inputs, Outputs);
            
            InstantiatePlatforms();

            List<Genotype> genPlayers = _playerPopulation.genetics;
            List<Phenotype> phPlayers = _playerPopulation.population;
            List<Genotype> genEnemies = _enemyPopulation.genetics;
            List<Phenotype> phsEnemies = _enemyPopulation.population;

            _gameManagers = (GameManager[])FindObjectsOfType(typeof(GameManager));
            _gameFinished = 0;
            
            foreach (var gm in _gameManagers)
            {
                gm.UpdateGameState(GameState.InProgress);
            }

            // Association de Neat aux object dans la scene
            for (int i = 0; i < populationSize; ++i)
            {
                _players[i].SetNeat(phPlayers[i], genPlayers[i]);
                _enemies[i].SetNeat(phsEnemies[i], genEnemies[i]);
            }
        }

        /// <summary>
        /// Ajout à la scene des platformes d'entrainement sous forme de grille
        /// </summary>

        private void NewGeneration()
        {
            _playerPopulation.NewGeneration();
            _enemyPopulation.NewGeneration();
            List<Genotype> genPlayers = _playerPopulation.genetics;
            List<Phenotype> phPlayers = _playerPopulation.population;
            List<Genotype> genEnemies = _enemyPopulation.genetics;
            List<Phenotype> phsEnemies = _enemyPopulation.population;
            for (int i = 0; i < populationSize; ++i)
            {
                _players[i].SetNeat(phPlayers[i], genPlayers[i]);
                _enemies[i].SetNeat(phsEnemies[i], genEnemies[i]);
            }
        }
        private void InstantiatePlatforms()
        {
            var gap = 70;
            var gridSize = 10;
            var maxX = gap * populationSize / gridSize / 2;
            var maxZ = gap * (gridSize - 1)/2;
            
            for (float i = 0; i < populationSize; ++i)
            {
                var x = Mathf.Floor(i / gridSize)  * gap - maxX;
                var z = i % gridSize * gap - maxZ;
                Instantiate(platformPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }

            var objP = GameObject.FindGameObjectsWithTag("Player");
            var objE = GameObject.FindGameObjectsWithTag("Enemy");
            if (objE.Length != objP.Length && objP.Length != populationSize)
            {
                throw new Exception("La taille de population correspond pas aux nombre d'objets instanciés");
            }
            for (int i = 0; i < objP.Length; ++i)
            {
                _players[i] = objP[i].gameObject.GetComponent<NeatAgent>();
                _enemies[i] = objE[i].gameObject.GetComponent<NeatAgent>();
            }

            _beginTime = Time.time;
        }

        public void GameFinished()
        {
            _gameFinished += 1;
            // Debug.Log(_gameFinished+" parties finies");
        }

        private void Update()
        {
            if (_gameFinished >= populationSize || Time.time > _beginTime + gameDuration)
            {
                _gameFinished = 0;
                NewGeneration();
                foreach (var gm in _gameManagers)
                {
                    gm.SetPositions();
                    gm.UpdateGameState(GameState.InProgress);
                }
                Debug.Log("Génération numéro " + _playerPopulation.GENERATION);
                _beginTime = Time.time;
            }

            if (Input.GetKey(KeyCode.A)) // gestion de la rapidité d'écoulement du temps
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 10;
            }

            if (Input.GetKeyDown(KeyCode.S) || _playerPopulation.GENERATION % 50 == 0) // enregistrement des IAs
            {
                _enemies[0].Save();
            }
        }
    }
}