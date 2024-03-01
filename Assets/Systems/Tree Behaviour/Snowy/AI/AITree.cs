using System.Linq;
using Snowy.Core.AI.Behaviours;
using UnityEngine;
using UnityEngine.AI;
using Snowy.Core.Base;

namespace Snowy.Core.AI
{
  [CreateAssetMenu(fileName = "AITree", menuName = "Snowy/AI/Tree")]
  public class AITree : BehaviourTree
  {
    [SerializeField] public string playerTag = "Player";
    protected NavMeshAgent _agent;
    
    public NavMeshAgent Agent
    {
      get { return _agent; }
    }

    public void TrySave()
    {
      if (Nodes.Length < 1)
      {
        // wait for 2 seconds before starting the tree
        var behaviour = ScriptableObject.CreateInstance(typeof(MainLoop)) as BehaviourNode;
        SetNodes(behaviour);
      }
    }

    public override void Start()
    {
      base.Start();
    }

    public void Setup(NavMeshAgent agent)
    {
      _agent = agent;
    }
    
    protected override void PreProcess()
    {
      SetPostandLevelOrders();

      mainIterator = new AIBehaviourIterator(_agent, this, 0);
      activeTimers = new Utility.UpdateList<Utility.Timer>();
      
      SetRootIteratorReferences();
    }
    
    public static AITree Clone(AITree sourceTree) {
      // The tree clone will be blank to start. We will duplicate blackboard and nodes.
      var cloneBt = CreateInstance<AITree>();
      cloneBt.name = sourceTree.name;

      if (sourceTree.blackboard)
      {
        cloneBt.blackboard = Instantiate(sourceTree.blackboard);
      }

      // Source tree nodes should already be in pre-order.
      cloneBt.SetNodes(sourceTree.Nodes.Select(n => Instantiate(n)));

      // Relink children and parents for the cloned nodes.
      int maxCloneNodeCount = cloneBt.allNodes.Length;
      for (int i = 0; i < maxCloneNodeCount; ++i)
      {
        BehaviourNode nodeSource = sourceTree.allNodes[i];
        BehaviourNode copyNode = GetInstanceVersion(cloneBt, nodeSource);

        if (copyNode.IsComposite())
        {
          var copyComposite = copyNode as Composite;
          copyComposite.SetChildren(
            Enumerable.Range(0, nodeSource.ChildCount())
              .Select(childIndex => GetInstanceVersion(cloneBt, nodeSource.GetChildAt(childIndex)))
              .ToArray());
        }

        else if (copyNode.IsDecorator() && nodeSource.ChildCount() == 1)
        {
          var copyDecorator = copyNode as Decorator;
          copyDecorator.SetChild(GetInstanceVersion(cloneBt, nodeSource.GetChildAt(0))); ;
        }
      }

      foreach (BehaviourNode node in cloneBt.allNodes)
      {
        node.OnCopy();
      }

      return cloneBt;
    }
    
    public string GetPlayerTag()
    {
      return playerTag;
    }
    
  }
}