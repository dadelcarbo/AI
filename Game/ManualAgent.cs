namespace Game
{
    public class ManualAgent : AgentBase
    {
        public ManualAgent(Model model) : base(model)
        {

        }
        public override string Name => "Manual";

        public override Action Decide()
        {
            return Action.NOP;
        }
    }
}