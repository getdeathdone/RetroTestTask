using System;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using DefaultNamespace.Projectile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Component
{
  public class Attack : ComponentBase, IAttackable, IUpdateVisual, IUpdate
  {
    public event Action<float> OnUpdateVisual;
    
    private const float RICOCHET_CHANCE_LOW_HEALTH = 1.0f;
    private const float NORMAL_RICOCHET_CHANCE = 0.1f;
    private const float PERCENT_OF_ATTACK_PRICE = 0.1f;

    private int _strength;
    private int _strengthMax;
    private int _attackPrice;
    private Health _health;
    private InputManager _inputManager;
    private bool IsLowHealth => _health.IsLowHealth();
    private float StrengthPercentage => (float)_strength / _strengthMax;
    private bool IsInputManagerAvailable => _inputManager != null;
    private bool IsAttack => IsInputManagerAvailable ? _inputManager.Attack : false;
    private bool IsAvailableUltimate => _strength >= _strengthMax;
    private bool IsAttackUltimate => IsAvailableUltimate && (IsInputManagerAvailable && _inputManager.UltimateAttack || false);

    public override void Initialize()
    {
      _health = ComponentOwner.GetAttachedComponent<Health>();
      _inputManager = (ComponentOwner as HeroPlayer)?.InputManager;
      _strength = ComponentOwner.HeroData.StrengthInit;
      _strengthMax = ComponentOwner.HeroData.StrengthMax;
      
      _attackPrice = (int)(_strengthMax * PERCENT_OF_ATTACK_PRICE);
      
      OnUpdateVisual?.Invoke(StrengthPercentage);
      
      IsInitialized = true;
    }

    public void Update()
    {
      if (IsAttack)
      {
        GetAttack();
      } else if (IsAttackUltimate)
      {
        GetAttack(AttackType.Ultimate);
      }
    }

    public void GetAttack (AttackType attackType = AttackType.None)
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