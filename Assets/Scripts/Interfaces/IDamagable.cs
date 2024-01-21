using System;

namespace DefaultNamespace.Interfaces
{
  public interface IDamagable
  {
    public event Action<DamageInfo> OnGetDamage;

    public void GetDamage(AttackInfo attackInfo);
  }
  
  public class DamageInfo : AttackInfo
  {
    private readonly IDamagable _receiver;      // тот кто получил
    public IDamagable Receiver => _receiver;

    public DamageInfo(int damage, AttackType attackType, IAttackable damageDealer) : base(damage, attackType, damageDealer)
    {}
    public DamageInfo (AttackInfo attackInfo, IDamagable receiver) : base(attackInfo.Damage, attackInfo.AttackType, attackInfo.DamageDealer)
    {
      _receiver = receiver;
    }
  }
}