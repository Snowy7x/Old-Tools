﻿
using Snowy.Core.Base;
using UnityEngine;

namespace Snowy.Core.Standard
{
  [BehaviourNode("Conditional/", "Condition")]
  public class Chance : ConditionalTask
  {
    [Tooltip("The probability that the condition succeeds.")]
    [Range(0f, 1f)]
    public float chance = 0.5f;

    public override bool Condition()
    {
      // Return true if the probability is within the range [0, chance];
      return Random.value <= chance;
    }
  }
}