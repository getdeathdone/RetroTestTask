using DefaultNamespace.Hero;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class PlayerMovement : Movement
  {
    private InputManager _inputManager;
    protected override Vector3 MovementDirection => _transform.TransformDirection(_inputManager.Direction);
    private float Rotate => _inputManager.RotateHorizontalInput;
    private bool UseSlowdown => _inputManager.UseRotateKeyboard;

    public override void Initialize()
    {
      _inputManager = ((HeroPlayer)ComponentOwner).InputManager;
      
      base.Initialize();
    }

    public override void Update()
    {
      base.Update();
      RotateUpdate();
    }
    
    private void RotateUpdate()
    {
      _transform.Rotate(Vector3.up, Rotate * InputManager.ROTATION_INPUT_SPEED * (UseSlowdown ? Time.fixedDeltaTime * InputManager.KEYBOARD_SLOWDOWN : 1));
    }

    protected override void OutOfBounds()
    {
      MoveAwayFromEnemies();
    }

    private void MoveAwayFromEnemies()
    {
      Vector3 nearestEnemy = _areaManager.GetNearestEnemy(_transform.position);
      
      _transform.position = GetSafePosition(nearestEnemy);
    }
    
    private Vector3 GetSafePosition(Vector3 enemyPosition)
    {
      Vector3 direction = (_transform.position - enemyPosition).normalized;
      Vector3 safePosition = CenterPoint + direction * Radius;

      return safePosition;
    }
  }
}