using UnityEngine;

namespace Systems.Procedural_Creation.Texture.Editor
{
    [UnityEditor.CustomEditor(typeof(TextureViewer))]
    public class TextureViewerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var textureViewer = target as TextureViewer;
            if (textureViewer == null) return;
            if (GUILayout.Button("Generate"))
            {
                textureViewer.Generate();
            }
        }
    }
}