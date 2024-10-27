using DefaultNamespace.Component;
using UnityEngine;

namespace DefaultNamespace.Calculator
{
    public class RicochetCalculator
    {
        private readonly Health _health;
        public const float RICOCHET_CHANCE_LOW_HEALTH = 1.0f;
        public const float NORMAL_RICOCHET_CHANCE = 0.1f;

        public RicochetCalculator(Health health)
        {
            _health = health;
        }

        public bool ShouldProjectileRicochet()
        {
            float ricochetChance = _health.IsLowHealth() ? RICOCHET_CHANCE_LOW_HEALTH : NORMAL_RICOCHET_CHANCE;
            return Random.Range(0f, 1f) <= ricochetChance;
        }
    }
}