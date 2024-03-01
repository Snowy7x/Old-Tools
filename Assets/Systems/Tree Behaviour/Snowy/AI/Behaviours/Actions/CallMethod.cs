using Snowy.Core.Base;

namespace Snowy.Core.AI.Behaviours
{
    [BehaviourNode("AI/Actions/", "SmallCross")]
    public class CallMethod : Task
    {
        public string methodName;
        
        public override Status Run()
        {
            try
            {
                Actor.SendMessage(methodName);
            }
            catch
            {
                // ignored
            }

            return Status.Success;
        }
    }
}