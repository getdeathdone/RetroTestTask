using UnityEngine;

namespace DefaultNamespace.Component
{
  public class AIRangeAttack : Attack
  {
    public const float DETECTION_RANGE = 6f;
    private const float SHOOT_INTERVAL = 1f;
    public const float ATTACK_RANGE = 4f;

    private float _shootTimer;
    private Transform _transform;

    public override void Initialize()
    {
      _shootTimer = 0;
      _transform = ComponentOwner.transform;
      
      base.Initialize();
    }

    public override void Update()
    {
      base.Update();
      
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
          Shoot(_transform.position, directionToPlayer.normalized, target);
        }

        _shootTimer = 0f;
      }
    }
  }
}