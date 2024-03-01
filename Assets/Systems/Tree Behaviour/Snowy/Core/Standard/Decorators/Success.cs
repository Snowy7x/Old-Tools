
using System.Text;
using Snowy.Core.Base;

namespace Snowy.Core.Standard
{
  /// <summary>
  /// Always returns success.
  /// </summary>
  [BehaviourNode("Decorators/", "SmallCheckmark")]
  public class Success : Decorator
  {
    public override Status Run()
    {
      return Status.Success;
    }

    public override void Description(StringBuilder builder)
    {
      builder.Append("Always succeed");
    }
  }
}