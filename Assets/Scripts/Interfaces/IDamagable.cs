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
    private readonly bool _isDeadReceiver;      // тот кто получил
    public IDamagable Receiver => _receiver;
    public bool IsDeadReceiver => _isDeadReceiver;

    public DamageInfo(int damage, AttackType attackType, IAttackable damageDealer) : base(damage, attackType, damageDealer)
    {}
    public DamageInfo (AttackInfo attackInfo, IDamagable receiver, bool isDeadReceiver) : base(attackInfo.Damage, attackInfo.AttackType, attackInfo.DamageDealer)
    {
      _receiver = receiver;
      _isDeadReceiver = isDeadReceiver;
    }
  }
}