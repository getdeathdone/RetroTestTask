using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
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
    }

    void Update()
    {
      transform.position += _velocity * Time.deltaTime;
      transform.forward = _velocity.normalized;

      RaycastHit closestHit = new RaycastHit();
      closestHit.distance = Mathf.Infinity;
      bool foundHit = false;

      Vector3 displacementSinceLastFrame = transform.position - _lastPosition;

      RaycastHit [] hits = Physics.SphereCastAll(_lastPosition, Radius(), displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, _hittableLayers);

      List<IDamagable> damagables = new List<IDamagable>();

      foreach (var hit in hits)
      {
        if (IsHitValid(hit, out damagables) && hit.distance < closestHit.distance)
        {
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

        OnHit(damagables);
      }

      Debug.DrawLine(_lastPosition, transform.position, Color.blue);

      _lastPosition = transform.position;
    }

    private bool IsHitValid (RaycastHit hit, out List<IDamagable> damagables)
    {
      damagables = new List<IDamagable>();

      if (_ignoredColliders.Contains(hit.collider))
      {
        return false;
      }

      if (!hit.collider.gameObject.TryGetComponent(out IDamagable damagable))
      {
        return false;
      }

      if (damagable is ComponentBase component)
      {
        damagables = component.ComponentOwner.GetInterfaceImplementations<IDamagable>();
      } else
      {
        damagables.Add(damagable);
      }

      return true;
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

        RaycastHit [] ricochetHits = Physics.SphereCastAll(transform.position, Radius(), ownerTransform.position - transform.position, RICOCHET_DISTANCE, _hittableLayers);

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

    private float Radius()
    {
      Vector3 capsuleColliderScale = _capsuleCollider.transform.localScale;
      return Mathf.Max(_capsuleCollider.radius * capsuleColliderScale.x, _capsuleCollider.radius * capsuleColliderScale.y, _capsuleCollider.radius * capsuleColliderScale.z);
    }
  }
}