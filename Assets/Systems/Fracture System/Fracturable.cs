using UnityEngine;
using Random = System.Random;

namespace Snowy.Fracture_System
{
    public class Fracturable : MonoBehaviour
    {
        [SerializeField] private Anchor anchor = Anchor.Bottom;
        [SerializeField] private int chunks = 500;
        [SerializeField] private float density = 50;
        [SerializeField] private float internalStrength = 100;
        
        [SerializeField] private Material insideMaterial;
        [SerializeField] private Material outsideMaterial;
        
        private Random rng = new Random();
        
        private void Start()
        {
            DoFracture();
            gameObject.SetActive(false);
        }
        
        public ChunkGraphManager DoFracture()
        {
            var seed = rng.Next();
            return Fracture.FractureGameObject(
                gameObject,
                anchor,
                seed,
                chunks,
                insideMaterial,
                outsideMaterial,
                internalStrength,
                density
            );
        }
    }
}