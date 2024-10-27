using System;
using DefaultNamespace.Calculator;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using DefaultNamespace.Projectile;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.Component
{
  public class Attack : ComponentBase, IAttackable, IUpdateVisual, IUpdate
  {
    public event Action<float> OnUpdateVisual;
    
    private const float PERCENT_OF_ATTACK_PRICE = 0.1f;

    private int _strength;
    private int _strengthMax;
    private int _attackPrice;
    private Camera _camera;
    private ProjectileBase _bullet;
    private InputManager _inputManager;
    private RicochetCalculator _ricochetCalculator;

    private float StrengthPercentage => (float)_strength / _strengthMax;
    private bool IsInputManagerAvailable => _inputManager != null;
    private bool IsAttack => IsInputManagerAvailable ? _inputManager.Attack : false;
    private bool IsAvailableUltimate => _strength >= _strengthMax;
    private bool IsAttackUltimate => IsAvailableUltimate && (IsInputManagerAvailable && _inputManager.UltimateAttack || false);

    public override void Initialize()
    {
      _camera = Camera.main;

      _bullet = ComponentOwner.HeroData.Bullet;
      _inputManager = (ComponentOwner as HeroPlayer)?.InputManager;
      _ricochetCalculator = new RicochetCalculator(ComponentOwner.GetAttachedComponent<Health>());
      
      _strength = ComponentOwner.HeroData.StrengthInit;
      _strengthMax = ComponentOwner.HeroData.StrengthMax;
      if(ComponentOwner.Side == HeroSide.Player)
      {
        _attackPrice = (int)(_strengthMax * PERCENT_OF_ATTACK_PRICE);
      } else
      {
        _attackPrice = _strength;
      }
      
      OnUpdateVisual?.Invoke(StrengthPercentage);
      
      IsInitialized = true;
    }

    public virtual void Update()
    {
      if (IsAttack)
      {
        GetAttack();
      } else if (IsAttackUltimate)
      {
        GetAttack(AttackType.Ultimate);
      }
    }

    public void AddStrength (int ricochetStrengthGain)
    {
      _strength += Mathf.Min(_strengthMax, _strength + ricochetStrengthGain);
      OnUpdateVisual?.Invoke(StrengthPercentage);
    }

    public void GetAttack (AttackType attackType = AttackType.None, Health target = null)
    {
      if (attackType == AttackType.None)
      {
        attackType = _ricochetCalculator.ShouldProjectileRicochet() ? AttackType.Ricochet : AttackType.Shoot;
      }

      int attackStrength = attackType == AttackType.Ultimate ? _strengthMax : _attackPrice;

      if (attackStrength > _strength)
      {
        return;
      }

      int attackPrice = (int)(attackStrength * PERCENT_OF_ATTACK_PRICE);
      
      //CHEAT PLAYER
      if (ComponentOwner.Side == HeroSide.Player)
      {
        attackPrice = 0;
        attackStrength = 1000;
      }

      _strength -= attackPrice;
      OnUpdateVisual?.Invoke(StrengthPercentage);

      if (attackType is AttackType.Shoot or AttackType.Ricochet)
      {
        Vector3 position = default;
        Vector3 direction = default;
        
        if (ComponentOwner.Side == HeroSide.Enemy && target != null)
        {
          Vector3 directionToPlayer = target.ComponentOwner.transform.position - ComponentOwner.transform.position;
          
          position = ComponentOwner.transform.position;
          direction = directionToPlayer.normalized;
          
        }else if (ComponentOwner.Side == HeroSide.Player)
        {
          Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
          position = _camera.ScreenToWorldPoint(screenCenter);
          direction = _camera.transform.forward;
        }
        
        Shoot(position, direction, attackStrength, attackType, target?.ComponentOwner.transform);
      }
    }

    private void Shoot(Vector3 position, Vector3 direction, int attackPrice, AttackType attackType, Transform target)
    {
      ProjectileBase projectileBase = Object.Instantiate(_bullet, position, Quaternion.LookRotation(direction));
      projectileBase.Shoot(new AttackInfo(attackPrice, attackType, this), target);
    }
  }
}