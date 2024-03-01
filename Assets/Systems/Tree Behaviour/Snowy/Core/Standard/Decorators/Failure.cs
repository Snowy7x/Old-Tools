
using System.Text;
using Snowy.Core.Base;

namespace Snowy.Core.Standard
{
  /// <summary>
  /// Alaways returns failure.
  /// </summary>
  [BehaviourNode("Decorators/", "SmallCross")]
  public class Failure : Decorator
  {
    public override Status Run()
    {
      return Status.Failure;
    }

    public override void Description(StringBuilder builder)
    {
      builder.Append("Always fail");
    }
  }
}