using Snowy.Core.Base;
using Snowy.CustomAttributes;
using UnityEngine;

namespace Snowy.Core.AI.Behaviours
{
    [BehaviourNode("AI/Checker/", "SmallCross")]
    public class Distance : ConditionalTask
    {
        // Show tags list to choose from
        [TagSelector]
        public string targetTag = "Player";
        public float distance = 1f;
        Transform target;

        public override void OnStart()
        {
            target = GetTarget();
            base.OnStart();
        }

        private Transform GetTarget() => GameObject.FindGameObjectWithTag(targetTag).transform;

        public override bool Condition()
        {
            if (target == null)
            {
                target = GetTarget();
                return false;
            }
            return Vector3.Distance(Actor.transform.position, target.transform.position) < distance;
        }
    }
}