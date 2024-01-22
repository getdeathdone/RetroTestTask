using Cysharp.Threading.Tasks;
using DefaultNamespace.Component.AI;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Hero
{
  public class HeroEnemy : HeroBase
  {
    public override HeroSide Side => HeroSide.Enemy;
    private NavMeshAgent _navMeshAgent;

    private async void Start()
    {
      await UniTask.WaitUntil(() => IsInitialized);
      
      _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

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

      if (Type == HeroType.EnemyBlue && _navMeshAgent != null)
      {
        // Draw detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_navMeshAgent.transform.position, AttackRange.DETECTION_RANGE);

        // Draw attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_navMeshAgent.transform.position, AttackRange.ATTACK_RANGE);
      }
    }
#endif
  }
}