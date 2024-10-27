using System;
using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class Health : ComponentBase, IDamagable, IDeath, IUpdateVisual
  {
    private const int LOW_HEALTH_PERCENTAGE = 15;
    public event Action<DamageInfo> OnGetDamage;
    public event Action<DamageInfo> OnDeath;
    public event Action<float> OnUpdateVisual;

    private bool _isAlive;
    private int _currentHealth;
    private int _maxHealth;
    public bool IsAlive => _isAlive;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    private float HealthPercentage => (float)_currentHealth / MaxHealth;

    public override void Initialize ()
    {
      _maxHealth = ComponentOwner.HeroData.Health;
      _currentHealth = _maxHealth;
      
      OnUpdateVisual?.Invoke(HealthPercentage);
      _isAlive = true;
      IsInitialized = true;
    }

    public void GetDamage (AttackInfo attackInfo)
    {
      if (!IsAlive)
      {
        return;
      }
      
      _currentHealth -= attackInfo.Damage;
      bool isDead = _currentHealth > 0;

      DamageInfo info = new DamageInfo(attackInfo, this, isDead);
      
      OnGetDamage?.Invoke(info);
      OnUpdateVisual?.Invoke(HealthPercentage);

      Death(info);
    }
    
    public bool IsLowHealth()
    {
      float healthPercentage = HealthPercentage * 100f;
      return healthPercentage < LOW_HEALTH_PERCENTAGE;
    }

    public void RestoreHealth (int healthToRestore)
    {
      _currentHealth = Mathf.Min(MaxHealth, _currentHealth += healthToRestore);
      OnUpdateVisual?.Invoke(HealthPercentage);
    }

    public void Death(DamageInfo damageInfo)
    {
      _isAlive = false;
      OnDeath?.Invoke(damageInfo);

      if (ComponentOwner.Side == HeroSide.Enemy)
      {
        ComponentOwner.SetActive(false);
      }
    }
  }
}