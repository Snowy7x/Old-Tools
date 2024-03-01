
using System;
using Snowy.Core.Base;
using UnityEngine;

namespace Snowy.Designer
{
  public static class EditorChangeNodeType
  {
    public static void ChangeType(BonsaiNode node, Type newType)
    {
      var newBehaviour = ScriptableObject.CreateInstance(newType) as BehaviourNode;
      node.SetBehaviour(newBehaviour, NodeIcon(newType));
    }

    private static Texture NodeIcon(Type behaviourType)
    {
      var prop = BonsaiEditor.GetNodeTypeProperties(behaviourType);
      return BonsaiPreferences.Texture(prop.texName);
    }
  }
}
