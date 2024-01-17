using System;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class Health : ComponentBase, IDamagable, IUpdateVisual
  {
    private const int LOW_HEALTH_PERCENTAGE = 15;

    public event Action<DamageInfo> OnGetDamage;

    public event Action<float> OnUpdateVisual;
    
    private int _currentHealth;
    private int _maxHealth;
    
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    public override void Initialize ()
    {
      _maxHealth = ComponentOwner.HeroData.Health;
      _currentHealth = _maxHealth;
      
      IsInitialized = true;
    }

    public void GetDamage (int damage, AttackType attackType, HeroBase damageDealer)
    {
      if (!ComponentOwner.IsAlive)
      {
        return;
      }
      
      _currentHealth -= damage;

      DamageInfo info = new DamageInfo(damage, attackType, damageDealer, this);
      
      OnGetDamage?.Invoke(info);
      OnUpdateVisual?.Invoke((float)_currentHealth / MaxHealth);

      if (_currentHealth > 0)
      {
        return;
      }

      ComponentOwner.Death(info);
    }
    
    public bool IsLowHealth()
    {
      float healthPercentage = (float)CurrentHealth / MaxHealth * 100f;
      return healthPercentage < LOW_HEALTH_PERCENTAGE;
    }

    public void RestoreHealth (int healthToRestore)
    {
      _currentHealth = Mathf.Min(MaxHealth, _currentHealth += healthToRestore);
    }
  }
}