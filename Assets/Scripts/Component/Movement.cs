using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public abstract class Movement : ComponentBase, IUpdate, IFixedUpdate
  {
    protected Transform _transform;
    
    private float _speed;
    private Rigidbody _rigidbody;

    protected virtual Vector3 MovementDirection => Vector3.zero;

    public override void Initialize()
    {
      _rigidbody = ComponentOwner.transform.GetComponent<Rigidbody>();
      _speed = ComponentOwner.HeroData.Speed;
      _transform = ComponentOwner.transform;

      IsInitialized = true;
    }

    public virtual void Update()
    {}

    public void FixedUpdate()
    {
      MovementUpdate();
    }

    private void MovementUpdate()
    {
      _rigidbody.AddForce(MovementDirection * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
  }
}