using System.Collections.Generic;
using Snowy.Core.Base;
using Snowy.CustomAttributes;
using Systems.Actor;
using UnityEngine;

public enum DamageType
{
    AnythingInRange,
    AnythingInFront,
    PlayerOnly,
    ActorOnly
}

namespace Snowy.Core.AI.Behaviours
{
    [BehaviourNode("AI/Actions/", "SmallCross")]
    public class Attack : Task
    {
        [TagSelector, Tooltip("The tag of the player.")]
        public string playerTag = "Player";
        [Tooltip("Whether to use the attack animation.")]
        public bool useAttackAnimation = true;
        [Tooltip("The type of damage to deal.")]
        public DamageType damageType = DamageType.AnythingInRange;
        [Tooltip("The amount of damage to deal.")]
        public float damage = 10f;
        [Tooltip("The range of the attack.")]
        public float range = 1f;
        [Tooltip("The cooldown between attacks.")]
        public float cooldown = 1f;
        
        private float _lastAttackTime;
        private IDamageable me;
        
        public override void OnStart()
        {
            base.OnStart();
            me = Actor.GetComponentInChildren<IDamageable>();
        }

        public override Status Run()
        {
            if (Time.time - _lastAttackTime < cooldown) return Status.Failure;
            
            List<IDamageable> damageables = new List<IDamageable>();
            switch (damageType)
            {
                case DamageType.AnythingInRange:
                    Collider[] entities = new Collider[100];
                    int count = Physics.OverlapSphereNonAlloc(Actor.transform.position, range, entities);
                    for (int i = 0; i < count; i++)
                    {
                        IDamageable damageable = entities[i].GetComponentInChildren<IDamageable>();
                        if (damageable != null) damageables.Add(damageable);
                    }

                    break;

                case DamageType.AnythingInFront:
                    Debug.LogWarning("Not implemented yet.");
                    break;

                case DamageType.PlayerOnly:
                    GameObject player = GameObject.FindGameObjectWithTag(playerTag);
                    if (player != null && Vector3.Distance(Actor.transform.position, player.transform.position) < range)
                    {
                        IDamageable damageable = player.GetComponentInChildren<IDamageable>();
                        if (damageable != null) damageables.Add(damageable);
                    }
                    else
                    {
                        Debug.LogWarning("Player not found or out of range.");
                    }

                    break;

                case DamageType.ActorOnly:
                    Collider[] actors = new Collider[100];
                    int c = Physics.OverlapSphereNonAlloc(Actor.transform.position, range, actors);
                    for (int i = 0; i < c; i++)
                    {
                        IDamageable damageable = actors[i].GetComponentInChildren<Actor>();
                        if (damageable != null) damageables.Add(damageable);
                    }

                    break;

            }
            
            
            if (damageables.Count > 0)
            {
                // if (useAttackAnimation) Actor.Animator.SetTrigger("Attack");
                foreach (IDamageable damageable in damageables)
                {
                    if (damageable == null || damageable == me) continue;
                    damageable.TakeDamage(damage);
                }
            }
            
            _lastAttackTime = Time.time;
            return Status.Success;
        }
    }
}