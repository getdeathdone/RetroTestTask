using System;
using DefaultNamespace.Hero;

namespace DefaultNamespace.Interfaces
{
  public interface IDamagable
  {
    public event Action<DamageInfo> OnGetDamage;

    public void GetDamage(int damage, AttackType attackType, HeroBase damageDealer);
  }

  public class DamageInfo
  {
    private readonly int _damage;
    private readonly AttackType _attackType;
    private readonly HeroBase _damageDealer; // тот кто нанес
    private readonly IDamagable _receiver;   // тот кто получил

    public DamageInfo(int damage, AttackType attackType, HeroBase damageDealer, IDamagable receiver)
    {
      _damage = damage;
      _attackType = attackType;
      _damageDealer = damageDealer;
      _receiver = receiver;
    }
    
    public int Damage => _damage;
    public AttackType AttackType => _attackType;
    public HeroBase DamageDealer => _damageDealer;
    public IDamagable Receiver => _receiver;
  }
}