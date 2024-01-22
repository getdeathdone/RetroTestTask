using DefaultNamespace.Hero;
using UnityEngine;

namespace DefaultNamespace.Component.AI
{
  public class AttackRange : Attack
  {
    public const float DETECTION_RANGE = 6f;
    private const float SHOOT_INTERVAL = 3f;
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
        HeroBase target = ComponentOwner.FindTarget(DETECTION_RANGE, ((HeroEnemy)ComponentOwner).PlayerMask);

        if (target != null)
        {
          float distance = Vector3.Distance(_transform.position, target.transform.position);
          if (distance <= ATTACK_RANGE)
          {
            return;
          }

          Health health = target.GetAttachedComponent<Health>();
          GetAttack(AttackType.Shoot, health);
          
          _shootTimer = 0f;
        }
      }
    }
  }
}