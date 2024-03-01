using UnityEngine;
using UnityEngine.UI;

namespace Systems.Procedural_Creation.Texture
{
    public class TextureViewer : MonoBehaviour
    {
        [SerializeField] Vector2Int size = new Vector2Int(256, 256);
        [SerializeField] float depth = 0.5f;
        [SerializeField] int seed = 0;
        
        // Shader properties
        [SerializeField] RawImage image;
        [SerializeField] ComputeShader computeShader;
        [SerializeField] RenderTexture _renderTexture;
        void Start()
        {
            Generate();
        }

        public void Generate()
        {
            _renderTexture = new RenderTexture(size.x, size.y, 0, RenderTextureFormat.ARGB32);
            _renderTexture.enableRandomWrite = true;
            _renderTexture.Create();

            var kernel = computeShader.FindKernel("CSMain");
            computeShader.SetInt("seed", seed);
            computeShader.SetInt("width", size.x);
            computeShader.SetInt("height", size.y);
            computeShader.SetFloat("depth", depth);
            computeShader.SetTexture(kernel, "Result", _renderTexture);
            computeShader.Dispatch(kernel, size.x / 8, size.y / 8, 1);
            
            image.texture = _renderTexture;
        }
    }
}