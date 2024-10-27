using System;

namespace DefaultNamespace.Interfaces
{
    public interface IDeath
    {
        public event Action<DamageInfo> OnDeath;
        
        public bool IsAlive { get; }
        public void Death(DamageInfo damageInfo);
    }
}