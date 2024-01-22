using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Component
{
  public class FlyKiller : ComponentBase, IUpdate
  {
    private Transform _transform;

    public override void Initialize()
    {
      ComponentOwner.OnCollisionEvent += CollisionEvent;
      
      Rigidbody rigidbody = ComponentOwner.GetComponent<Rigidbody>();
      if (rigidbody != null)
      {
        rigidbody.isKinematic = true;
      }
      
      var navMeshAgent = ComponentOwner.GetComponentInChildren<NavMeshAgent>();
      if (navMeshAgent != null)
      {
        navMeshAgent.enabled = false;
      }

      _transform = ComponentOwner.transform;
      IsInitialized = true;
    }

    public void Update()
    {
      throw new System.NotImplementedException();
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
      health.GetDamage(new AttackInfo(15, AttackType.Lethal, null));
      
      ComponentOwner.Death();
    }
  }
}