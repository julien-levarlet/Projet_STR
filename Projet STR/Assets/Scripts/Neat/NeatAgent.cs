using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Met en place les méthodes utiles pour l'algorithme NEAT.
    /// </summary>
    public class NeatAgent : AgentController
    {
        [SerializeField] protected string saveFile;
        protected Phenotype neatPhenotype;
        protected Genotype neatGenotype;

        /// <summary>
        /// Associe le joueur à son réseau 
        /// </summary>
        /// <param name="ph"></param>
        /// <param name="gen"></param>
        public void SetNeat(Phenotype ph, Genotype gen)
        {
            neatPhenotype = ph;
            neatGenotype = gen;
        }

        public override bool AttackCondition()
        {
            throw new System.NotImplementedException();
        }

        protected override float GetInputVertical()
        {
            throw new System.NotImplementedException();
        }

        protected override float GetInputHorizontal()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Ajout de la valeur en paramètre à la fitnesse du modele
        /// </summary>
        public override void Reward(float reward)
        {
            neatGenotype.fitness += reward;
        }
    }
}