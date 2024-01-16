using System;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace.Component
{
  public class Health : ComponentBase, IDamagable, IUpdateVisual
  {
    public event Action OnDeath;
    public event Action<int> OnGetDamage;
    public event Action<float> OnUpdateVisual;
    
    private int _currentHealth;
    private int _maxHealth;
    
    public int MaxHealth => _maxHealth;

    public override void Initialize ()
    {
      _maxHealth = ComponentOwner.HeroData.Health;
      _currentHealth = _maxHealth;
      
      IsInitialized = true;
    }

    public void GetDamage (int damage)
    {
      _currentHealth -= damage;
      
      OnGetDamage?.Invoke(damage);
      OnUpdateVisual?.Invoke((float)_currentHealth / MaxHealth);

      if (_currentHealth > 0)
      {
        return;
      }

      OnDeath?.Invoke();
    }
  }
}