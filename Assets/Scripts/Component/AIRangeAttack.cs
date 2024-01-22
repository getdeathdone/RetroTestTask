using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class AIRangeAttack : ComponentBase, IUpdate
  {
    public const float DETECTION_RANGE = 6f;
    private const float SHOOT_INTERVAL = 1f;
    public const float ATTACK_RANGE = 4f;

    private float _shootTimer;
    private Attack _attack;
    private Transform _transform;

    public override void Initialize()
    {
      _attack = ComponentOwner.GetAttachedComponent<Attack>();
      _transform = ComponentOwner.transform;

      _shootTimer = 0;

      IsInitialized = true;
    }

    public void Update()
    {
      _shootTimer += Time.deltaTime;

      if (_shootTimer >= SHOOT_INTERVAL)
      {
        Transform target = ComponentOwner.FindTarget(DETECTION_RANGE);

        if (target != null)
        {
          float distance = Vector3.Distance(_transform.position, target.position);
          if (distance <= ATTACK_RANGE)
          {
            return;
          }
          
          Vector3 directionToPlayer = target.position - _transform.position;
          _attack.Shoot(_transform.position, directionToPlayer.normalized);
        }

        _shootTimer = 0f;
      }
    }
  }
}