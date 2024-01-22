using DefaultNamespace.Component;
using DefaultNamespace.Component.AI;
using UnityEngine;

namespace DefaultNamespace.Hero
{
  public class HeroEnemy : HeroBase
  {
    public override HeroSide Side => HeroSide.Enemy;

    public void DestroyEnemy()
    {
      Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
      if (Type == HeroType.EnemyRed)
      {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FlyKiller.DETECTION_RANGE);
      }

      if (Type == HeroType.EnemyBlue)
      {
        // Draw detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRange.DETECTION_RANGE);

        // Draw attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange.ATTACK_RANGE);
      }
    }
#endif
  }
}