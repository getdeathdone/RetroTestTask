using System;
using DefaultNamespace.Hero;

namespace DefaultNamespace.Projectile
{
  public class ProjectileBase
  {
    public event Action OnShoot;
    
    private readonly int _damage;
    private readonly AttackType _attackType;
    private readonly HeroBase _damageDealer; 

    public ProjectileBase(int damage, AttackType attackType, HeroBase damageDealer)
    {
      _damage = damage;
      _attackType = attackType;
      _damageDealer = damageDealer;
    }

    public void Shoot()
    {
      OnShoot?.Invoke();
    }
  }
}