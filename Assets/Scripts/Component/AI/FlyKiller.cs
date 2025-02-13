using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Component.AI
{
  public class FlyKiller : ComponentBase, IUpdate, IAttackable
  {
    public const float DETECTION_RANGE = 10f;
    private const float INITIAL_UPWARD_SPEED = 5f;
    private const float HOVER_TIME = 2f;
    private const float ATTACK_SPEED = 10f;
    private const float FLY_DURATION = 5f;
    private const float MAXIMUM_HEIGHT = 5f;
    private const float BOOM_DISTANCE = 0.1f;

    private Transform _transform;
    private bool _isHovering;
    private float _startTime;
    private int _damage;
    private float _speed;
    private Transform Transform => _transform;

    public override void Initialize()
    {
      Rigidbody rigidbody = ComponentOwner.GetComponent<Rigidbody>();

      if (rigidbody != null)
      {
        rigidbody.isKinematic = true;
      }

      NavMeshAgent navMeshAgent = ComponentOwner.GetComponentInChildren<NavMeshAgent>();

      if (navMeshAgent != null)
      {
        navMeshAgent.enabled = false;
      }

      _isHovering = true;
      _startTime = Time.time;
      _transform = ComponentOwner.transform;
      _damage = ComponentOwner.HeroData.StrengthInit;
      _speed = ComponentOwner.HeroData != null ? ComponentOwner.HeroData.Speed : 0f;

      IsInitialized = true;
    }

    public void Update()
    {
      float elapsed = Time.time - _startTime;

      if (_isHovering)
      {
        if (elapsed < FLY_DURATION)
        {
          Vector3 newPosition = Transform.position + Vector3.up * INITIAL_UPWARD_SPEED * Time.deltaTime;

          newPosition.y = Mathf.Min(newPosition.y, MAXIMUM_HEIGHT);

          Transform.position = newPosition;
        } else
        {
          _isHovering = false;
          _startTime = Time.time;
        }
      } else
      {
        if (elapsed < HOVER_TIME)
        {} else
        {
          HeroBase player = ComponentOwner.FindTarget(DETECTION_RANGE, ((HeroEnemy)ComponentOwner).PlayerMask);

          if (player != null)
          {
            Vector3 playerDirection = (player.transform.position - Transform.position).normalized;
            Transform.Translate(playerDirection * (_speed * ATTACK_SPEED) * Time.deltaTime);

            var distanceToPlayer = Vector3.Distance(_transform.position, player.transform.position);

            if (distanceToPlayer < BOOM_DISTANCE)
            {
              Health health = player.GetAttachedComponent<Health>();
              GetAttack(AttackType.Lethal, health);
              
              ComponentOwner.SetActive(false);
            }
          }
        }
      }
    }

    public void GetAttack (AttackType attackType, Health target)
    {
      target?.GetDamage(new AttackInfo(_damage, attackType, this));
    }
  }
}