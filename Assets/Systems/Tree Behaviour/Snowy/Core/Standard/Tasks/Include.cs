using System.Text;
using Snowy.Core.Base;
using Snowy.Core.AI;
using UnityEngine;

namespace Snowy.Core.Standard
{
  [BehaviourNode("Tasks/", "TreeIcon")]
  public class Include : Task
  {
    [Tooltip("The sub-tree to run when this task executes.")]
    public BehaviourTree subtreeAsset;

    public BehaviourTree RunningSubTree { get; private set; }

    public override void OnStart()
    {
      if (subtreeAsset)
      {
        if (subtreeAsset is AITree)
        {
          RunningSubTree = AITree.Clone(subtreeAsset as AITree);
          (RunningSubTree as AITree)?.Setup((Iterator as AIBehaviourIterator)?.GetAgent());
        }
        else
        {
          RunningSubTree = BehaviourTree.Clone(subtreeAsset);
        }
        RunningSubTree.actor = Actor;
        RunningSubTree.Start();
      }
    }

    public override void OnEnter()
    {
      RunningSubTree.BeginTraversal();
    }

    public override void OnExit()
    {
      if (RunningSubTree.IsRunning())
      {
        RunningSubTree.Interrupt();
      }
    }

    public override Status Run()
    {
      if (RunningSubTree)
      {
        RunningSubTree.Update();
        return RunningSubTree.IsRunning()
          ? Status.Running
          : RunningSubTree.LastStatus();
      }

      // No tree was included. Just fail.
      return Status.Failure;
    }

    public override void Description(StringBuilder builder)
    {
      if (subtreeAsset)
      {
        builder.AppendFormat("Include {0}", subtreeAsset.name);
      }
      else
      {
        builder.Append("Tree not set");
      }
    }
  }
}