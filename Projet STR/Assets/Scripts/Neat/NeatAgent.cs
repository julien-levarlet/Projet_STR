using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Met en place les méthodes utiles pour l'algorithme NEAT.
    /// </summary>
    public class NeatAgent : AgentController
    {
        private const string SavePath = "Assets/Resources/";
        [SerializeField] private string sceneName = "Nom de la scene";
        [SerializeField] private bool isTraining;
        [SerializeField] private Transform victoryTarget;
        private Phenotype _neatPhenotype;
        private Genotype _neatGenotype;
        private float[] _actions = {0f, 0f, 0f};

        protected override void Start()
        {
            base.Start();
            if (!isTraining)
            {
                Load(); // initialise le genetype et phénotype par ceux présent en mémoire
            }
        }

        /// <summary>
        /// Associe le joueur à son réseau 
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="gen"></param>
        public void SetNeat(Phenotype ph, Genotype gen)
        {
            _neatPhenotype = ph;
            _neatGenotype = gen;
            _neatGenotype.fitness = 0.1f; // évite les problèmes de fitnesse nulle
        }

        public void GetNeatOutput()
        {
            if (_neatPhenotype == null) return;
            
            // création de l'entrée du réseau
            float[] input =
            {
                transform.eulerAngles.y, transform.localPosition.x, transform.localPosition.z, target.localPosition.x, target.localPosition.z,
                victoryTarget.localPosition.x, victoryTarget.localPosition.z
            };
            _actions = _neatPhenotype.Propagate(input); // on récupère la sortie
        }

        public override bool AttackCondition()
        {
            return _actions[2] > 0.5f;
        }

        protected override float GetInputVertical()
        {
            GetNeatOutput();
            return 2*(_actions[0])-1;
        }

        protected override float GetInputHorizontal()
        {
            return 2*_actions[1]-1;
        }

        /// <summary>
        /// Ajout de la valeur en paramètre à la fitnesse du modele
        /// </summary>
        public override void Reward(float reward)
        {
            _neatGenotype.fitness += reward;
        }
        
        /// <summary>
        /// Ecriture en mémoire des caractéristique du réseau de l'agent, pour l'utiliser plus tard.
        /// Code adapté de l'exemple du monopoly
        /// </summary>
        public void Save()
        {
            StringBuilder sb = new StringBuilder(100);
            int vertices = _neatGenotype.vertices.Count;

            for (int k = 0; k < vertices; k++)
            {
                sb.Append(_neatGenotype.vertices[k].index.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(_neatGenotype.vertices[k].type.ToString() + ',');
            }

            sb.Append('#');

            int edges = _neatGenotype.edges.Count;

            for (int k = 0; k < edges; k++)
            {
                sb.Append(_neatGenotype.edges[k].source.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(_neatGenotype.edges[k].destination.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(_neatGenotype.edges[k].weight.ToString(CultureInfo.InvariantCulture) + ','); // normalement pas de problème de virgule ici
                sb.Append(_neatGenotype.edges[k].enabled.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(_neatGenotype.edges[k].innovation.ToString(CultureInfo.InvariantCulture) + ',');
            }

            var saveFile = SavePath + sceneName + ".txt";
            using (StreamWriter sw = new StreamWriter(saveFile))
            {
                sw.AutoFlush = true; 
                sw.WriteLine(sb);
            }
        }

        /// <summary>
        /// Charge le réseau enregistré pour une utilisation réelle.
        /// Code adapté de l'exemple du monopoly
        /// </summary>
        public void Load()
        {
            _neatGenotype = new Genotype();
            _neatPhenotype = new Phenotype();
            
            var saveFile = SavePath + sceneName + ".txt";
            string network = "";
            using (StreamReader sr = new StreamReader(saveFile))
            {
                network = sr.ReadLine();
            }

            if (network == null)
                throw new Exception("Fichier de sauvegarde non trouvé");
            
            string[] nparts = network.Split('#');

            string verts = nparts[0];
            string[] vparts = verts.Split(',');

            for (int j = 0; j < vparts.GetLength(0) - 1; j += 2)
            {
                int index = int.Parse(vparts[j],CultureInfo.InvariantCulture);
                VertexInfo.EType type = (VertexInfo.EType)Enum.Parse(typeof(NEAT.VertexInfo.EType), vparts[j + 1]);

                _neatGenotype.AddVertex(type, index);
            }

            string edges = nparts[1];
            string[] eparts = edges.Split(',');

            for (int j = 0; j < eparts.GetLength(0) - 1; j += 5)
            {
                int source = int.Parse(eparts[j],CultureInfo.InvariantCulture);
                int destination = int.Parse(eparts[j + 1],CultureInfo.InvariantCulture);
                float weight = float.Parse(eparts[j + 2],CultureInfo.InvariantCulture);
                bool b = bool.Parse(eparts[j + 3]);
                int innovation = int.Parse(eparts[j + 4],CultureInfo.InvariantCulture);

                _neatGenotype.AddEdge(source, destination, weight, b, innovation);
            }
            
            _neatPhenotype.InscribeGenotype(_neatGenotype);
            _neatPhenotype.ProcessGraph();
        }
    }
}