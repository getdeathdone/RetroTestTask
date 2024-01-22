using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Projectile
{
  public class ProjectileBase : MonoBehaviour
  {
    private const float MAX_LIFE_TIME = 5f;
    private const float RICOCHET_DISTANCE = 10f;
    public event Action OnShoot;

    private readonly List<Collider> _ignoredColliders = new List<Collider>();

    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private LayerMask _hittableLayers;
    [SerializeField]
    private CapsuleCollider _capsuleCollider;

    private bool _isShoot;
    private bool _isRicochet;
    private Vector3 _velocity;
    private Vector3 _lastPosition;
    private AttackInfo _attackInfo;

    private void Awake()
    {
      Destroy(gameObject, MAX_LIFE_TIME);
    }

    public void Shoot (AttackInfo attackInfo)
    {
      _attackInfo = attackInfo;
      
      _lastPosition = transform.position;
      _velocity = transform.forward * _speed;
      _isRicochet = attackInfo.AttackType == AttackType.Ricochet;

      var heroBase = (_attackInfo.DamageDealer as ComponentBase)?.ComponentOwner;

      if (heroBase != null)
      {
        Collider [] ownerColliders = heroBase.GetComponentsInChildren<Collider>();
        _ignoredColliders.AddRange(ownerColliders);
      }

      OnShoot?.Invoke();
      _isShoot = true;
    }

    void Update()
    {
      if (!_isShoot)
      {
        return;
      }
      
      transform.position += _velocity * Time.deltaTime;
      transform.forward = _velocity.normalized;

      RaycastHit closestHit = new RaycastHit();
      closestHit.distance = Mathf.Infinity;
      bool foundHit = false;

      Vector3 displacementSinceLastFrame = transform.position - _lastPosition;

      RaycastHit [] hits = Physics.SphereCastAll(_lastPosition, _capsuleCollider.radius, displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, _hittableLayers);

      List<IDamagable> damagablesValid = new List<IDamagable>();

      foreach (var hit in hits)
      {
        Debug.Log("Hit: " + hit.collider.gameObject.name);
        
        if (IsHitValid(hit, out List<IDamagable> damagables) && hit.distance < closestHit.distance)
        {
          damagablesValid.AddRange(damagables);
          foundHit = true;
          closestHit = hit;
        }
      }

      if (foundHit)
      {
        if (closestHit.distance <= 0f)
        {
          closestHit.point = transform.position;
          closestHit.normal = -transform.forward;
        }

        OnHit(damagablesValid);
      }

      Debug.DrawLine(_lastPosition, transform.position, Color.blue, 10f);

      _lastPosition = transform.position;
    }

    private bool IsHitValid (RaycastHit hit, out List<IDamagable> damagables)
    {
      damagables = new List<IDamagable>();

      if (_ignoredColliders.Contains(hit.collider))
      {
        return false;
      }

      if (hit.collider.gameObject.TryGetComponent(out HeroBase heroBase))
      {
        damagables = heroBase.GetInterfaceImplementations<IDamagable>();
      }
      else if (hit.collider.gameObject.TryGetComponent(out IDamagable damagable))
      {
        damagables.Add(damagable);
      }
      
      return damagables.Count > 0;
    }

    private void OnHit (List<IDamagable> damagables)
    {
      foreach (var damagable in damagables)
      {
        damagable.GetDamage(_attackInfo);
      }

      if (_isRicochet && damagables.Count > 0)
      {
        _isRicochet = false;
        var ownerTransform = ((ComponentBase)damagables[0]).ComponentOwner.transform;

        RaycastHit [] ricochetHits = Physics.SphereCastAll(transform.position, _capsuleCollider.radius, ownerTransform.position - transform.position, RICOCHET_DISTANCE, _hittableLayers);

        bool foundNewEnemy = false;

        foreach (var hit in ricochetHits)
        {
          if (!IsHitValid(hit, out List<IDamagable> newDamagables))
          {
            continue;
          }

          if (ContainsAny(damagables, newDamagables))
          {
            continue;
          }

          foundNewEnemy = true;
          break;
        }

        if (foundNewEnemy)
        {
          Vector3 reflectionDirection = Vector3.Reflect(_velocity.normalized, ownerTransform.position - transform.position);
          
          _velocity = reflectionDirection * _speed;
          transform.position += _velocity * Time.deltaTime * 0.1f;
          transform.forward = _velocity.normalized;
        } else
        {
          Destroy(gameObject);
        }
      } else
      {
        Destroy(gameObject);
      }
      
      bool ContainsAny(List<IDamagable> list1, List<IDamagable> list2)
      {
        foreach (var item1 in list1)
        {
          foreach (var item2 in list2)
          {
            if (item1.Equals(item2))
            {
              return true;
            }
          }
        }
        return false;
      }
    }
  }
}