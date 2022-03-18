using SharpNeat.Phenomes;

namespace Neat
{
    public class NeatEnemy : NeatAgent
    {
        protected override void Start()
        {
            base.Start();
        }
        
        protected override float GetInputVertical()
        {
            throw new System.NotImplementedException();
        }

        protected override float GetInputHorizontal()
        {
            throw new System.NotImplementedException();
        }

        public override void ActivateUnit(IBlackBox blackBox)
        {
            throw new System.NotImplementedException();
        }

        public override void DeactivateUnit()
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
        {
            throw new System.NotImplementedException();
        }

        protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
        {
            throw new System.NotImplementedException();
        }

        public override float GetFitness()
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleIsActiveChanged(bool newIsActive)
        {
            throw new System.NotImplementedException();
        }
    }
}
