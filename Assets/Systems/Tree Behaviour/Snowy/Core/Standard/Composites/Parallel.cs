using Snowy.Core.Base;
namespace Snowy.Core.Standard
{
  /// <summary>
  /// Parallel node which succeeds if all its children succeed.
  /// </summary>
  [BehaviourNode("Composites/", "Parallel")]
  public class Parallel : ParallelComposite
  {
    public override Status Run()
    {
      if (IsAnyChildWithStatus(Status.Failure))
      {
        return Status.Failure;
      }

      if (AreAllChildrenWithStatus(Status.Success))
      {
        return Status.Success;
      }

      RunChildBranches();

      // Parallel iterators still running.
      return Status.Running;
    }
  }
}