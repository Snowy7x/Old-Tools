
using Snowy.Core.Base;

namespace Snowy.Core.Standard
{
  /// <summary>
  /// Re-traversing the child until it returns failure.
  /// </summary>
  [BehaviourNode("Decorators/", "RepeatArrow")]
  public class UntilFailure : Decorator
  {
    public override Status Run()
    {
      Status s = Iterator.LastChildExitStatus.GetValueOrDefault(Status.Failure);

      if (s == Status.Failure)
      {
        return Status.Success;
      }

      // Retraverse child.
      Iterator.Traverse(child);

      return Status.Running;
    }
  }
}