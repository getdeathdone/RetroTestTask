using DefaultNamespace.Component;
using UnityEngine;

namespace DefaultNamespace.Interfaces
{
  public interface IAttackable
  {
    public void GetAttack(AttackType attackType, Health target);
  }
  
  public class AttackInfo
  {
    private readonly int _damage;
    private readonly AttackType _attackType;
    private readonly IAttackable _damageDealer; // тот кто нанес

    public int Damage => _damage;
    public AttackType AttackType => _attackType;
    public IAttackable DamageDealer => _damageDealer;

    public AttackInfo (int damage, AttackType attackType, IAttackable damageDealer)
    {
      _damage = damage;
      _attackType = attackType;
      _damageDealer = damageDealer;
    }
  }
}