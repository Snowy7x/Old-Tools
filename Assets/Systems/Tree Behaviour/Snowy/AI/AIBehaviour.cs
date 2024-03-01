using Snowy.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Snowy.Core.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIBehaviour : MonoBehaviour
    {
        private NavMeshAgent _agent;
        
        [SerializeField]
        public AITree treeBlueprint;
        
        internal AITree TreeInstance;

        void Awake()
        {
            if (!TryGetComponent(out _agent))
            {
                Debug.LogError("The NavMeshAgent is not set for " + gameObject);
            }
            
            if (treeBlueprint)
            {
                TreeInstance = AITree.Clone(treeBlueprint);
                TreeInstance.actor = gameObject;
                TreeInstance.Setup(_agent);
            }
            else
            {
                Debug.LogError("The behaviour tree is not set for " + gameObject);
            }
        }
        
        void Start()
        {
            TreeInstance.Start();
            TreeInstance.BeginTraversal();
        }
        
        void Update()
        {
            TreeInstance.Update();
        }
        
        void OnDestroy()
        {
            Destroy(TreeInstance);
        }

        /// <summary>
        /// The tree instance running in game.
        /// </summary>
        public AITree Tree
        {
            get { return TreeInstance; }
        }
    }
}