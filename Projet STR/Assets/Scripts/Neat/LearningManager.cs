using System;
using System.Collections.Generic;
using UnityEngine;

namespace NEAT
{
    public class LearningManager: MonoBehaviour
    {
        private const int Inputs = 6; // notre position, position du joueur et position de l'objectif
        private const int Outputs = 3;
        private const int PopulationSize = 50;

        [SerializeField] private GameObject platformPrefab;
        private NeatAgent[] _players;
        private NeatAgent[] _enemies;
        private Population _playerPopulation;
        private Population _enemyPopulation;
        
        private void Start()
        {
            // Création des singletons
            Crossover.Initialise();
            Mutation.Initialise();
            NetworkFactory.Initialise();

            _playerPopulation = new Population();
            _enemyPopulation = new Population();

            _players = new NeatAgent[PopulationSize];
            _enemies = new NeatAgent[PopulationSize];
            
            // mise en place de la population
            _playerPopulation.GenerateBasePopulation(PopulationSize, Inputs, Outputs);
            _enemyPopulation.GenerateBasePopulation(PopulationSize, Inputs, Outputs);
            
            InstantiatePlatforms();

            List<Genotype> genPlayers = _playerPopulation.genetics;
            List<Phenotype> phPlayers = _playerPopulation.population;
            List<Genotype> genEnemies = _enemyPopulation.genetics;
            List<Phenotype> phsEnemies = _enemyPopulation.population;

            // Association de Neat aux object dans la scene
            for (int i = 0; i < PopulationSize; ++i)
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
            for (int i = 0; i < PopulationSize; ++i)
            {
                _players[i].SetNeat(phPlayers[i], genPlayers[i]);
                _enemies[i].SetNeat(phsEnemies[i], genEnemies[i]);
            }
            
        }
        private void InstantiatePlatforms()
        {
            var gap = 50;
            var gridSize = 10;
            var maxX = gap * PopulationSize / gridSize / 2;
            var maxZ = gap * (gridSize - 1)/2;
            
            for (float i = 0; i < PopulationSize; ++i)
            {
                var x = Mathf.Floor(i / gridSize)  * gap - maxX;
                var z = i % gridSize * gap - maxZ;
                Instantiate(platformPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }

            var objP = GameObject.FindGameObjectsWithTag("Player");
            var objE = GameObject.FindGameObjectsWithTag("Enemy");
            if (objE.Length != objP.Length && objP.Length != PopulationSize)
            {
                throw new Exception("La taille de population correspond pas aux nombre d'objets instanciés");
            }
            for (int i = 0; i < objP.Length; ++i)
            {
                _players[i] = objP[i].gameObject.GetComponent<NeatAgent>();
                _enemies[i] = objE[i].gameObject.GetComponent<NeatAgent>();
            }
        }
    }
}