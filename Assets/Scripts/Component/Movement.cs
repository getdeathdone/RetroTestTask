using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public abstract class Movement : ComponentBase, IUpdate, IFixedUpdate
  {
    protected Transform _transform;
    
    private float _speed;
    private Rigidbody _rigidbody;
    protected AreaManager _areaManager;

    protected virtual Vector3 MovementDirection => Vector3.zero;
    protected float Radius => _areaManager.Radius;
    protected Vector3 CenterPoint => _areaManager.CenterPoint;

    public override void Initialize()
    {
      _rigidbody = ComponentOwner.transform.GetComponent<Rigidbody>();
      _speed = ComponentOwner.HeroData.Speed;
      _transform = ComponentOwner.transform;
      _areaManager = ComponentOwner.AreaManager;

      IsInitialized = true;
    }

    public virtual void Update()
    {
      CheckNearEdge();
    }

    public void FixedUpdate()
    {
      MovementUpdate();
    }

    private void MovementUpdate()
    {
      _rigidbody.AddForce(MovementDirection * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void CheckNearEdge()
    {
      float distanceToCenter = Vector3.Distance(_transform.position, CenterPoint);

      if (distanceToCenter > Radius)
      {
        OutOfBounds();
      }
    }

    protected virtual void OutOfBounds()
    {}
  }
}