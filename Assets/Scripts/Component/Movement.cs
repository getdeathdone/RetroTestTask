using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class Movement : ComponentBase, IUpdate, IFixedUpdate
  {
    private Transform _transform;
    private Rigidbody _rigidbody;
    private InputManager _inputManager;
    
    private float _speed;

    public override void Initialize()
    {
      _transform = ComponentOwner.transform;
      _rigidbody = ComponentOwner.transform.GetComponent<Rigidbody>();
      _inputManager = (ComponentOwner as HeroPlayer)?.InputManager;
      _speed = ComponentOwner.HeroData.Speed;

      IsInitialized = true;
    }

    public virtual void Update()
    {
      RotateUpdate();
    }

    public void FixedUpdate()
    {
      MovementUpdate();
    }

    private void RotateUpdate()
    {
      float angle = 0;
      
      if (_inputManager != null)
      {
        _transform.Rotate(Vector3.up, _inputManager.RotateHorizontal);

      }
      
      _transform.Rotate(Vector3.up, angle);
    }

    private void MovementUpdate()
    {
      Vector3 movementDirection = default;

      if (_inputManager != null)
      {
        movementDirection = _transform.TransformDirection(_inputManager.Direction);
      }
      
      _rigidbody.AddForce(movementDirection * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
  }
}