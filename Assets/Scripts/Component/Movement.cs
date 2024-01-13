using DefaultNamespace.Handler;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class Movement : ComponentBase, IUpdate, IFixedUpdate
  {
    private float _speed;
    private float _rotationSpeed;
    
    private Transform _transform;
    private Rigidbody _rigidbody;
    private AreaManager _areaManager;
    private InputHandler _inputHandler;

    public override void Initialize()
    {
      _speed = ComponentOwner.Speed;
      _rotationSpeed = ComponentOwner.RotationSpeed;
      _transform = ComponentOwner.transform;
      _rigidbody = ComponentOwner.Rigidbody;
      _areaManager = ComponentOwner.AreaManager;
      _inputHandler = ComponentOwner.InputHandler;

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
      Vector3 movementDirection = _transform.TransformDirection(_inputHandler.Direction);

      _rigidbody.AddForce(movementDirection * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void RotateUpdate()
    {
      if (_inputHandler.RotateLeftHeld)
      {
        _transform.Rotate(Vector3.up, -_rotationSpeed * Time.fixedDeltaTime);
      }

      if (_inputHandler.RotateRightHeld)
      {
        _transform.Rotate(Vector3.up, _rotationSpeed * Time.fixedDeltaTime);
      }
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