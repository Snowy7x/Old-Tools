using UnityEngine.AI;
using Snowy.Core.Base;

namespace Snowy.Core.AI
{
  /// <summary>
  /// A special iterator to handle traversing a behaviour tree.
  /// </summary>
  public sealed class AIBehaviourIterator : BehaviourIterator
  {
    private NavMeshAgent _agent;
    private AITree AITree => tree as AITree;
    
    public AIBehaviourIterator(NavMeshAgent agent, BehaviourTree tree, int levelOffset) : base(tree, levelOffset)
    {
      _agent = agent;
    }
    
    public NavMeshAgent GetAgent()
    {
      if (_agent == null)
      {
        _agent = AITree.Agent;
      }
      
      return _agent;
    }
    
    public string GetPlayerTag()
    {
      return AITree.playerTag;
    }
  }
}