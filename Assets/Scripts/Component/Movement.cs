using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class Movement : ComponentBase, IUpdate, IFixedUpdate
  {
    protected Transform _transform;
    
    private float _speed;
    private float _rotationSpeed;
    private Rigidbody _rigidbody;
    private AreaManager _areaManager;

    protected virtual Vector3 MovementDirection => Vector3.zero;
    protected virtual int Rotate => 0;

    public override void Initialize()
    {
      _speed = ComponentOwner.HeroData.Speed;
      _rotationSpeed = ComponentOwner.HeroData.RotationSpeed;
      _transform = ComponentOwner.transform;
      _rigidbody = ComponentOwner.Rigidbody;
      _areaManager = ComponentOwner.AreaManager;

      IsInitialized = true;
    }

    public void Update()
    {
      ClampMovementToBounds();
    }

    public void FixedUpdate()
    {
      MovementUpdate();
      RotateUpdate();
    }

    private void MovementUpdate()
    {
      _rigidbody.AddForce(MovementDirection * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void RotateUpdate()
    {
      _transform.Rotate(Vector3.up, Rotate * _rotationSpeed * Time.fixedDeltaTime);
    }

    private void ClampMovementToBounds()
    {
      float radius = _areaManager.Radius;
      Vector3 centerPoint = _areaManager.CenterPoint;
      
      float distanceToCenter = Vector3.Distance(_transform.position, centerPoint);

      if (distanceToCenter > radius)
      {
        Vector3 normalizedDirection = (_transform.position - centerPoint).normalized;
        
        Vector3 newPosition = centerPoint + normalizedDirection * radius;
        
        newPosition.x = Mathf.Clamp(newPosition.x, centerPoint.x - radius, centerPoint.x + radius);
        newPosition.z = Mathf.Clamp(newPosition.z, centerPoint.z - radius, centerPoint.z + radius);
        
        _transform.position = newPosition;
      }
    }
  }
}