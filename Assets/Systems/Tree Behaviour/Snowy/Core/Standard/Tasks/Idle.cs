using System.Text;
using Snowy.Core.Base;

namespace Snowy.Core.Standard
{
  /// <summary>
  /// Always returns running.
  /// </summary>
  [BehaviourNode("Tasks/", "Hourglass")]
  public class Idle : Task
  {
    public override Status Run()
    {
      return Status.Running;
    }

    public override void Description(StringBuilder builder)
    {
      builder.Append("Run forever");
    }
  }
}

