using System.Text;
using Snowy.Core;
using Snowy.Core.Base;

namespace Snowy.Core.AI.Behaviours
{
    [BehaviourNode("AI/", "RepeatArrow")]
    public sealed class MainLoop : Decorator
    {
    

        public override void OnEnter()
        {
        }

        public override Status Run()
        {
            // Infinite loop always returns running and always traverses the child.
            Iterator.Traverse(child);
            return Status.Running;
        }

        public override void Description(StringBuilder builder)
        {
            builder.Append("Loop forever");
        }

    }
}