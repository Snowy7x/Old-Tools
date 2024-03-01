
using Snowy.Core.Standard;
using UnityEditor;
using UnityEngine;

namespace Snowy.Designer
{
  [CustomEditor(typeof(Interruptor))]
  public class InterruptorInspector : BehaviourNodeInspector
  {
    private readonly GUIStyle linkStyle = new GUIStyle();

    protected override void OnEnable()
    {
      base.OnEnable();
      linkStyle.alignment = TextAnchor.MiddleCenter;
      linkStyle.fontStyle = FontStyle.Bold;
    }

    protected override void OnBehaviourNodeInspectorGUI()
    {
      EditorGUILayout.BeginVertical();
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Shift + Click to link Interruptables", linkStyle);
      EditorGUILayout.EndVertical();
    }
  }
}