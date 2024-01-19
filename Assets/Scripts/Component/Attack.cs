using System;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Projectile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Component
{
  public class Attack : ComponentBase, IAttackable, IUpdateVisual
  {
    public event Action<float> OnUpdateVisual;
    
    private const float RICOCHET_CHANCE_LOW_HEALTH = 1.0f;
    private const float NORMAL_RICOCHET_CHANCE = 0.1f;
    private const float PERCENT_OF_ATTACK_PRICE = 0.1f;

    private int _strength;
    private int _strengthMax;
    private int _attackPrice;
    private Health _health;
    private bool IsLowHealth => _health.IsLowHealth();
    private float StrengthPercentage => (float)_strength / _strengthMax;
    private bool IsAvailableUltimate => _strength >= _strengthMax;

    public override void Initialize()
    {
      _health = ComponentOwner.GetAttachedComponent<Health>();
      _strength = ComponentOwner.HeroData.StrengthInit;
      _strengthMax = ComponentOwner.HeroData.StrengthMax;
      
      _attackPrice = (int)(_strengthMax * PERCENT_OF_ATTACK_PRICE);
      
      OnUpdateVisual?.Invoke(StrengthPercentage);
      
      IsInitialized = true;
    }

    public void GetAttackUltimate()
    {
      if (!IsAvailableUltimate)
      {
        return;
      }
      
      GetAttack(AttackType.Ultimate);
    }

    public void GetAttack(AttackType attackType = AttackType.None)
    {
      if (attackType == AttackType.None)
      {
        attackType = ShouldProjectileRicochet() ? AttackType.Ricochet : AttackType.Normal;
      }

      int attackPrice = attackType == AttackType.Ultimate ? _strengthMax : _attackPrice;
      
      if (attackPrice > _strength)
      {
        return;
      }
      
      _strength -= attackPrice;
      OnUpdateVisual?.Invoke(StrengthPercentage);

      ProjectileBase projectileBase = new ProjectileBase(attackPrice, attackType, ComponentOwner);
      projectileBase.Shoot();
    }

    public void AddStrength (int ricochetStrengthGain)
    {
     _strength += Mathf.Min(_strengthMax, _strength + ricochetStrengthGain);
     OnUpdateVisual?.Invoke(StrengthPercentage);
    }

    private bool ShouldProjectileRicochet()
    {
      float ricochetChance = IsLowHealth ? RICOCHET_CHANCE_LOW_HEALTH : NORMAL_RICOCHET_CHANCE;
      return Random.Range(0f, 1f) <= ricochetChance;
    }
  }
}