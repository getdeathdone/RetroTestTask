using DefaultNamespace.Interfaces;
using DefaultNamespace.Projectile;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class Attack : ComponentBase, IAttackable
  {
    private const float RICOCHET_CHANCE_LOW_HEALTH = 1.0f;
    private const float NORMAL_RICOCHET_CHANCE = 0.1f;

    private int _strength;
    private int _strengthMax;
    private int _attackPrice;
    private bool _isAvailableUltimate;
    private Health _health;
    private bool IsLowHealth => _health.IsLowHealth();
    
    public override void Initialize()
    {
      _health = ComponentOwner.GetAttachedComponent<Health>();
      _strengthMax = ComponentOwner.HeroData.Strength;
      _strength = _strengthMax;
      
      _attackPrice = (int)(_strengthMax * 0.1f);
      
      IsInitialized = true;
    }

    public void GetAttackUltimate()
    {
      if (!_isAvailableUltimate)
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

      ProjectileBase projectileBase = new ProjectileBase(attackPrice, attackType, ComponentOwner);
      projectileBase.Shoot();
    }

    public void AddStrength (int ricochetStrengthGain)
    {
     _strength += Mathf.Min(_strengthMax, _strength + ricochetStrengthGain);
    }

    private bool ShouldProjectileRicochet()
    {
      float ricochetChance = IsLowHealth ? RICOCHET_CHANCE_LOW_HEALTH : NORMAL_RICOCHET_CHANCE;
      return Random.Range(0f, 1f) <= ricochetChance;
    }
  }
}