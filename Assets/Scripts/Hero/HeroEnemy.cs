using DefaultNamespace.Component;
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
    }
#endif
  }
}