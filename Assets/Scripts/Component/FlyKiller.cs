using System.Linq;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Component
{
  public class FlyKiller : ComponentBase, IUpdate
  {
    public const float DETECTION_RANGE = 10f;
    private const float INITIAL_UPWARD_SPEED = 5f;
    private const float HOVER_TIME = 2f;
    private const float ATTACK_SPEED = 10f;
    private const float FLY_DURATION = 5f;
    private const float MAXIMUM_HEIGHT = 5f;

    private Transform _transform;
    private bool _isHovering;
    private float _startTime;
    private int _damage;

    public override void Initialize()
    {
      ComponentOwner.OnCollisionEvent += CollisionEvent;

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

      IsInitialized = true;
    }

    public void Update()
    {
      float elapsed = Time.time - _startTime;

      if (_isHovering)
      {
        if (elapsed < FLY_DURATION)
        {
          Vector3 newPosition = _transform.position + Vector3.up * INITIAL_UPWARD_SPEED * Time.deltaTime;

          newPosition.y = Mathf.Min(newPosition.y, MAXIMUM_HEIGHT);

          _transform.position = newPosition;
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
          Transform player = FindTarget();

          if (player != null)
          {
            Vector3 playerDirection = (player.position - _transform.position).normalized;
            _transform.Translate(playerDirection * ATTACK_SPEED * Time.deltaTime);
          }
        }
      }
    }

    private Transform FindTarget()
    {
      Collider [] hitColliders = Physics.SphereCastAll(_transform.position, DETECTION_RANGE, _transform.forward).Select(hit => hit.collider).ToArray();

      foreach (Collider collider in hitColliders)
      {
        if (collider.TryGetComponent(out HeroBase heroBase))
        {
          if (ComponentOwner.Side != heroBase.Side)
          {
            return collider.transform;
          }
        }
      }

      return null;
    }

    private void CollisionEvent (bool value, Collision other)
    {
      if (!value)
      {
        return;
      }

      if (!other.gameObject.TryGetComponent(out HeroBase heroBase))
      {
        return;
      }

      if (ComponentOwner.Side == heroBase.Side)
      {
        return;
      }

      ComponentOwner.OnCollisionEvent -= CollisionEvent;

      Health health = heroBase.GetAttachedComponent<Health>();
      health.GetDamage(new AttackInfo(_damage, AttackType.Lethal, null));

      ComponentOwner.Death();
    }
  }
}