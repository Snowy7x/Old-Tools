using Snowy.Core.Base;
using Snowy.CustomAttributes;
using UnityEngine;

namespace Snowy.Core.AI.Behaviours
{
    [BehaviourNode("AI/Checker/", "SmallCross")]
    public class CanSee : ConditionalTask
    {
        [TagSelector] public string targetTag = "Player";
        public float distance = 1f;

        public override bool Condition()
        {
            RaycastHit[] hits = new RaycastHit[10];
            if (Physics.SphereCastNonAlloc(Actor.transform.position, distance, Actor.transform.forward, hits,
                    distance) > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit.collider == null)
                    {
                        continue;
                    }
                    if (hit.collider.CompareTag(targetTag))
                    {
                        Vector3 offset = new Vector3(0, 0.25f, 0);
                        Vector3 targetPos = hit.transform.position + offset;
                        // Shoot a raycast to check if there is a wall between the actor and the target
                        Debug.DrawRay(Actor.transform.position, targetPos - Actor.transform.position,
                            Color.red, 1f);
                        if (Physics.Raycast(Actor.transform.position, targetPos - Actor.transform.position,
                            out var raycastHit, distance))
                        {
                            if (raycastHit.collider.CompareTag(targetTag))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}