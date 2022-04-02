
namespace NEAT
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

        public override bool AttackCondition()
        {
            throw new System.NotImplementedException();
        }
    }
}
