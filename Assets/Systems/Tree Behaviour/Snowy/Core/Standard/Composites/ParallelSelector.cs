
namespace Snowy.Core.Standard
{
  [BehaviourNode("Composites/", "ParallelSelector")]
  public class ParallelSelector : Parallel
  {
    public override Status Run()
    {
      if (IsAnyChildWithStatus(Status.Success))
      {
        return Status.Success;
      }

      if (AreAllChildrenWithStatus(Status.Failure))
      {
        return Status.Failure;
      }

      // Process the sub-iterators.
      RunChildBranches();

      // Parallel iterators still running.
      return Status.Running;
    }
  }
}