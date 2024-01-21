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
    public event Action OnShoot;
    
    private readonly List<Collider> _ignoredColliders = new List<Collider>();

    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private LayerMask _hittableLayers;
    [SerializeField]
    private CapsuleCollider _capsuleCollider;

    private AttackInfo _attackInfo;
    private float _shootTime;
    private Vector3 _velocity;
    private Vector3 _lastPosition;

    private void Awake()
    {
      Destroy(gameObject, MAX_LIFE_TIME);
    }

    public void Shoot (AttackInfo attackInfo)
    {
      _attackInfo = attackInfo;
      
      _shootTime = Time.time;
      _lastPosition = transform.position;
      _velocity = transform.forward * _speed;

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

      Destroy(gameObject);
    }

    private float Radius()
    {
      Vector3 capsuleColliderScale = _capsuleCollider.transform.localScale;
      return Mathf.Max(_capsuleCollider.radius * capsuleColliderScale.x, _capsuleCollider.radius * capsuleColliderScale.y, _capsuleCollider.radius * capsuleColliderScale.z);
    }
  }
}