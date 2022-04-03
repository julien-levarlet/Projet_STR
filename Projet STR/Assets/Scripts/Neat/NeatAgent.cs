using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Met en place les méthodes utiles pour l'algorithme NEAT.
    /// </summary>
    public abstract class NeatAgent : AgentController
    {
        [SerializeField] protected string saveFile;
        protected Phenotype neat;
        
        /// <summary>
        /// Associe le joueur à son réseau 
        /// </summary>
        /// <param name="ph"></param>
        public void SetNeat(Phenotype ph)
        {
            neat = ph;
        }

        /// <summary>
        /// Ajout de la valeur en paramètre à la fitnesse du modele
        /// </summary>
        public void Reward()
        {
            
        }
    }
}