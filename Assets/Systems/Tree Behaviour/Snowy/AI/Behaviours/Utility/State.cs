using System.Text;
using Snowy.Core.Base;
using UnityEngine;

namespace Snowy.Core.AI.Behaviours.Utility
{
  [BehaviourNode("AI/Utility/", "TreeIcon")]
  public class State : Task
  {
    [Tooltip("The sub-tree to run when this task executes.")]
    public SubTree subtreeAsset;

    public BehaviourTree RunningSubTree { get; private set; }

    public override void OnStart()
    {
      if (subtreeAsset)
      {
        if (subtreeAsset is AITree || subtreeAsset is SubTree)
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
      if (!RunningSubTree)
      {
        Debug.LogWarning("No tree was included in the State node.");
        return;
      }
      
      RunningSubTree.BeginTraversal();
    }

    public override void OnExit()
    {
      if (!RunningSubTree)
      {
        Debug.LogWarning("No tree was included in the State node.");
        return;
      }
      
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