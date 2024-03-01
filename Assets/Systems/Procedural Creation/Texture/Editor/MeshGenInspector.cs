using UnityEngine;

namespace Systems.Procedural_Creation.Texture.Editor
{
    [UnityEditor.CustomEditor(typeof(MeshGen))]
    public class MeshGenInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var meshGen = target as MeshGen;
            if (meshGen == null) return;
            if (GUILayout.Button("Create Single Mesh"))
            {
                meshGen.CreateSingleMesh();
            }
            if (GUILayout.Button("Create"))
            {
                meshGen.Create();
            }
            
            if (GUILayout.Button("Create With Shader"))
            {
                meshGen.Create(true);
            }
        }
    }
}