using UnityEngine;

namespace NEAT
{
    /// <summary>
    /// Met en place les méthodes utiles pour l'algorithme NEAT.
    /// </summary>
    public class NeatAgent : AgentController
    {
        [SerializeField] protected string saveFile;
        [SerializeField] private Transform victoryTarget;
        protected Phenotype neatPhenotype;
        protected Genotype neatGenotype;
        private float[] actions = {0f, 0f, 0f};

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

        public void getNeatOutput()
        {
            if (neatPhenotype == null) return;
            
            float[] input =
            {
                transform.position.x, transform.position.z, target.position.x, target.position.z,
                victoryTarget.position.x, victoryTarget.position.z
            };
            actions = neatPhenotype.Propagate(input);
        }

        public override bool AttackCondition()
        {
            return actions[2] > 0;
        }

        protected override float GetInputVertical()
        {
            getNeatOutput();
            return actions[0];
        }

        protected override float GetInputHorizontal()
        {
            return actions[1];
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