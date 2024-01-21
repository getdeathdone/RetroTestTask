using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Projectile
{
  public abstract class ProjectileBase : MonoBehaviour
  {
    private const float MAX_LIFE_TIME = 5f;
    public event Action OnShoot;

    [SerializeField]
    private Rigidbody _rigidbody;

    private AttackInfo _attackInfo;
    private readonly List<Collider> _ignoredColliders = new List<Collider>();

    private void Awake()
    {
      Destroy(gameObject, MAX_LIFE_TIME);
    }

    public void Shoot (AttackInfo attackInfo)
    {
      _attackInfo = attackInfo;

      var heroBase = (_attackInfo.DamageDealer as ComponentBase)?.ComponentOwner;

      if (heroBase != null)
      {
        Collider [] ownerColliders = heroBase.GetComponentsInChildren<Collider>();
        _ignoredColliders.AddRange(ownerColliders);
      }

      OnShoot?.Invoke();
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

      if (damagable == null)
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
  }
}