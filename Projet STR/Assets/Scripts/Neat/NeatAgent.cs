using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Met en place les m�thodes utiles pour l'algorithme NEAT.
    /// </summary>
    public class NeatAgent : AgentController
    {
        protected string saveFile = "Assets/Resources/save.txt";
        [SerializeField] private bool isTraining;
        [SerializeField] private Transform victoryTarget;
        protected Phenotype neatPhenotype;
        protected Genotype neatGenotype;
        private float[] actions = {0f, 0f, 0f};

        protected override void Start()
        {
            base.Start();
            if (!isTraining)
            {
                Load(); // initialise le genetype et ph�notype par ceux pr�sent en m�moire
            }
        }

        /// <summary>
        /// Associe le joueur � son r�seau 
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="gen"></param>
        public void SetNeat(Phenotype ph, Genotype gen)
        {
            neatPhenotype = ph;
            neatGenotype = gen;
            neatGenotype.fitness = 0.1f; // �vite les probl�mes de fitnesse nulle
        }

        public void GetNeatOutput()
        {
            if (neatPhenotype == null) return;
            
            // cr�ation de l'entr�e du r�seau
            float[] input =
            {
                transform.eulerAngles.y, transform.localPosition.x, transform.localPosition.z, target.localPosition.x, target.localPosition.z,
                victoryTarget.localPosition.x, victoryTarget.localPosition.z
            };
            actions = neatPhenotype.Propagate(input); // on r�cup�re la sortie
        }

        public override bool AttackCondition()
        {
            return actions[2] > 0.5f;
        }

        protected override float GetInputVertical()
        {
            GetNeatOutput();
            return 2*(actions[0])-1;
        }

        protected override float GetInputHorizontal()
        {
            return 2*actions[1]-1;
        }

        /// <summary>
        /// Ajout de la valeur en param�tre � la fitnesse du modele
        /// </summary>
        public override void Reward(float reward)
        {
            neatGenotype.fitness += reward;
        }
        
        /// <summary>
        /// Ecriture en m�moire des caract�ristique du r�seau de l'agent, pour l'utiliser plus tard.
        /// Code adapt� de l'exemple du monopoly
        /// </summary>
        public void Save()
        {
            StringBuilder sb = new StringBuilder(100);
            int vertices = neatGenotype.vertices.Count;

            for (int k = 0; k < vertices; k++)
            {
                sb.Append(neatGenotype.vertices[k].index.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(neatGenotype.vertices[k].type.ToString(CultureInfo.InvariantCulture) + ',');
            }

            sb.Append('#');

            int edges = neatGenotype.edges.Count;

            for (int k = 0; k < edges; k++)
            {
                sb.Append(neatGenotype.edges[k].source.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(neatGenotype.edges[k].destination.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(neatGenotype.edges[k].weight.ToString(CultureInfo.InvariantCulture) + ','); // normalement pas de probl�me de virgule ici
                sb.Append(neatGenotype.edges[k].enabled.ToString(CultureInfo.InvariantCulture) + ',');
                sb.Append(neatGenotype.edges[k].innovation.ToString(CultureInfo.InvariantCulture) + ',');
            }

            using (StreamWriter sw = new StreamWriter(saveFile))
            {
                sw.AutoFlush = true; 
                sw.WriteLine(sb);
            }
        }

        /// <summary>
        /// Charge le r�seau enregistr� pour une utilisation r�elle.
        /// Code adapt� de l'exemple du monopoly
        /// </summary>
        public void Load()
        {
            neatGenotype = new Genotype();
            neatPhenotype = new Phenotype();
            
            string network = "";
            using (StreamReader sr = new StreamReader(saveFile))
            {
                network = sr.ReadLine();
            }

            if (network == null)
                throw new Exception("Fichier de sauvegarde non trouv�");
            
            string[] nparts = network.Split('#');

            string verts = nparts[0];
            string[] vparts = verts.Split(',');

            for (int j = 0; j < vparts.GetLength(0) - 1; j += 2)
            {
                int index = int.Parse(vparts[j],CultureInfo.InvariantCulture);
                VertexInfo.EType type = (VertexInfo.EType)Enum.Parse(typeof(NEAT.VertexInfo.EType), vparts[j + 1]);

                neatGenotype.AddVertex(type, index);
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

                neatGenotype.AddEdge(source, destination, weight, b, innovation);
            }
            
            neatPhenotype.InscribeGenotype(neatGenotype);
            neatPhenotype.ProcessGraph();
        }
    }
}