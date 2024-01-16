using System;

namespace DefaultNamespace.Interfaces
{
  public interface IDamagable
  {
    public event Action<int> OnGetDamage;

    public void GetDamage(int damage);
  }
}