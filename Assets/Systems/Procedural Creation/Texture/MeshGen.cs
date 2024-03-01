using UnityEngine;

namespace Systems.Procedural_Creation.Texture
{
    public class MeshGen : MonoBehaviour
    {
        [SerializeField] Vector2Int meshSize = new(256, 256);
        [SerializeField] Vector2Int chunkSize = new(16, 16);
        [SerializeField] Material material;
        
        // Shader Try:
        [SerializeField] private ComputeShader meshShader;

        public void CreateSingleMesh()
        {
            if (TryGetComponent<MeshFilter>(out MeshFilter filter))
            {
                Mesh mesh = CreateMesh(filter.sharedMesh, meshSize);
                filter.sharedMesh = mesh;
            }
        }
        
        public void Create(bool withShader = false)
        {
            // Remove all children
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            // Calculate how many chunks we need and each chunk's size
            var chunkCount = new Vector2Int(meshSize.x / chunkSize.x, meshSize.y / chunkSize.y);
            var chunkSizeRemainder = new Vector2Int(meshSize.x % chunkSize.x, meshSize.y % chunkSize.y);
            var chunkSizes = new Vector2Int[chunkCount.x * chunkCount.y];
            for (int i = 0; i < chunkSizes.Length; i++)
            {
                chunkSizes[i] = chunkSize;
            }
            // Add the remainder as a new chunk
            if (chunkSizeRemainder.x > 0)
            {
                var newChunkSizes = new Vector2Int[chunkSizes.Length + chunkCount.y];
                chunkSizes.CopyTo(newChunkSizes, 0);
                for (int i = 0; i < chunkCount.y; i++)
                {
                    newChunkSizes[chunkSizes.Length + i] = new Vector2Int(chunkSizeRemainder.x, chunkSize.y);
                }
                chunkSizes = newChunkSizes;
                chunkCount.x++;
            }
            
            // Create new game objects for each chunk
            var chunks = new GameObject[chunkSizes.Length];
            // for each gameObject add a mesh filter and mesh renderer and calculate the mesh
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i] = new GameObject($"Chunk {i}");
                chunks[i].transform.parent = transform;
                chunks[i].transform.localPosition = Vector3.zero;
                chunks[i].transform.localRotation = Quaternion.identity;
                chunks[i].transform.localScale = Vector3.one;
                chunks[i].AddComponent<MeshFilter>();
                chunks[i].AddComponent<MeshRenderer>();
                chunks[i].GetComponent<MeshRenderer>().material = material;
                chunks[i].GetComponent<MeshFilter>().sharedMesh = withShader ? CreateMeshWithShader(new Mesh(), chunkSizes[i]) : CreateMesh(new Mesh(), chunkSizes[i]);
                
                // Calculate the position of the chunk
                var chunkPosition = new Vector3(
                    (i % chunkCount.x) * chunkSize.x,
                    0,
                    (i / chunkCount.x) * chunkSize.y
                );
                chunks[i].transform.localPosition = chunkPosition;
                
                // Calculate the UVs
                var mesh = chunks[i].GetComponent<MeshFilter>().sharedMesh;
                var uvs = new Vector2[mesh.vertices.Length];
                for (int j = 0; j < uvs.Length; j++)
                {
                    uvs[j] = new Vector2(
                        mesh.vertices[j].x / meshSize.x,
                        mesh.vertices[j].z / meshSize.y
                    );
                }
                mesh.uv = uvs;
            }
        }

        public Mesh CreateMesh(Mesh mesh, Vector2Int size)
        {
            if (mesh == null)
            {
                Debug.Log("Mesh is null");
                return null;
            }
            mesh.Clear();
            
            Vector3[] vertices = new Vector3[(size.x + 1) * (size.y + 1)];
            
            for (int i = 0, y = 0; y <= size.y; y++)
            {
                for (int x = 0; x <= size.x; x++, i++)
                {
                    vertices[i] = new Vector3(x, 0, y);
                }
            }
            
            int[] triangles = new int[size.x * size.y * 6];
            
            for (int ti = 0, vi = 0, y = 0; y < size.y; y++, vi++)
            {
                for (int x = 0; x < size.x; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + size.x + 1;
                    triangles[ti + 5] = vi + size.x + 2;
                }
            }
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            
            mesh.RecalculateNormals();
            
            return mesh;
        }
        
        public Mesh CreateMeshWithShader(Mesh mesh, Vector2Int size)
        {
            if (mesh == null)
            {
                Debug.Log("Mesh is null");
                return null;
            }
            mesh.Clear();
            
            // Set up the buffers
            var vertices = new ComputeBuffer((size.x + 1) * (size.y + 1), sizeof(float) * 3);
            var triangles = new ComputeBuffer(size.x * size.y * 6, sizeof(int));
            
            // Use the compute shader to calculate 
            var kernel = meshShader.FindKernel("CSMain");
            meshShader.SetInt("width", size.x);
            meshShader.SetInt("height", size.y);
            meshShader.SetBuffer(kernel, "Vertices", vertices);
            meshShader.SetBuffer(kernel, "Triangles", triangles);
            meshShader.Dispatch(kernel, Mathf.Max(1, size.x / 8), Mathf.Max(1,size.y / 8), 1); 
            
            // Get the data from the buffers
            var verticesArray = new Vector3[(size.x + 1) * (size.y + 1)];
            vertices.GetData(verticesArray);
            var trianglesArray = new int[size.x * size.y * 6];
            triangles.GetData(trianglesArray);
            
            
            // Debug the data
            
            Debug.Log("Triangles Count: " + trianglesArray.Length);
            foreach (var triangle in trianglesArray)
            {
                Debug.Log(triangle);
            }

            try
            {
                // Set the data to the mesh
                mesh.vertices = verticesArray;
                mesh.triangles = trianglesArray;

                mesh.RecalculateNormals();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            
            // Release the buffers
            vertices.Release();
            triangles.Release();
            
            return mesh;
        }
    }
}