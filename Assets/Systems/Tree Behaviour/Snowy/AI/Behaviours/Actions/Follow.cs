using Snowy.Core.Base;
using Snowy.CustomAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Snowy.Core.AI.Behaviours
{
    [BehaviourNode("AI/Actions/", "SmallCross")]
    public class Follow : Decorator
    {
        [TagSelector]
        public string targetTag = "Player";
        
        public float reachDistance = 1f;
        public float loseDistance = 1f;
        public float speed = 1f;
        
        private NavMeshAgent _agent;
        private Transform target;

        public override void OnStart()
        {
            GetAgent();
            base.OnStart();
        }

        public override void OnEnter()
        {
            target = GameObject.FindGameObjectWithTag(targetTag).transform;
            Debug.Log("Target: " + target);
            base.OnEnter();
        }

        private void GetAgent()
        {
            AIBehaviourIterator aiBehaviourIterator = Iterator as AIBehaviourIterator;
            if (aiBehaviourIterator == null)
            {
                return;
            }
            
            _agent = aiBehaviourIterator.GetAgent();
        }

        public override Status Run()
        {
            if (_agent == null)
            {
                GetAgent();
                return Status.Failure;
            }
            
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag(targetTag).transform;
                Stop();
                return Status.Failure;
            }
            
            var distance = Vector3.Distance(Actor.transform.position, target.transform.position);
            if (distance <= reachDistance)
            {
                Stop();
                return Status.Success;
            }
            
            if (distance > loseDistance)
            {
                Stop();
                return Status.Failure;
            }
            
            _agent.isStopped = false;
            
            _agent.SetDestination(target.transform.position);
            _agent.speed = speed;
            
            return Status.Running;
        }

        void Stop()
        {
            _agent.isStopped = true;
            _agent.SetDestination(Actor.transform.position);
            _agent.ResetPath();
            _agent.speed = 0f;
        }
    }
}