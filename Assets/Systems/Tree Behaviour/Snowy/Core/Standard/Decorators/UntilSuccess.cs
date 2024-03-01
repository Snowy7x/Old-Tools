
using Snowy.Core.Base;

namespace Snowy.Core.Standard
{
  /// <summary>
  /// Keep re-traversing children until the child return success.
  /// </summary>
  [BehaviourNode("Decorators/", "RepeatArrow")]
  public class UntilSuccess : Decorator
  {
    public override Status Run()
    {
      Status s = Iterator.LastChildExitStatus.GetValueOrDefault(Status.Success);

      if (s == Status.Success)
      {
        return Status.Success;
      }

      // Retraverse child.
      Iterator.Traverse(child);

      return Status.Running;
    }
  }
}