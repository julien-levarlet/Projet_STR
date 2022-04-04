using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Met en place les m�thodes utiles pour l'algorithme NEAT.
    /// </summary>
    public abstract class NeatAgent : AgentController
    {
        [SerializeField] protected string saveFile;
        protected Phenotype neatPhenotype;
        protected Genotype neatGenotype;

        /// <summary>
        /// Associe le joueur � son r�seau 
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

        /// <summary>
        /// Ajout de la valeur en param�tre � la fitnesse du modele
        /// </summary>
        public override void Reward(int reward)
        {
            
        }
    }
}