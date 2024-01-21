using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Component
{
  public class Movement : ComponentBase, IUpdate, IFixedUpdate
  {
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private InputManager _inputManager;

    public override void Initialize()
    {
      _transform = ComponentOwner.transform;
      _inputManager = (ComponentOwner as HeroPlayer)?.InputManager;
      _navMeshAgent = ComponentOwner.transform.GetComponent<NavMeshAgent>();
      _navMeshAgent.speed = (ComponentOwner as HeroPlayer)?.HeroData.Speed ?? 0f;

      IsInitialized = true;
    }

    public void Update()
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
        angle = _inputManager.RotateHorizontal;
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
      
      _navMeshAgent.Move(movementDirection * _navMeshAgent.speed * Time.fixedDeltaTime);
    }
  }
}