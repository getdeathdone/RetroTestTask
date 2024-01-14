using DefaultNamespace.Hero;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class InputMovement : Movement
  {
    private InputManager _inputManager;
    protected override Vector3 MovementDirection => _transform.TransformDirection(_inputManager.Direction);
    protected override int Rotate => _inputManager.RotateLeftHeld && !_inputManager.RotateRightHeld ? -1 : !_inputManager.RotateLeftHeld && _inputManager.RotateRightHeld ? 1 : 0;
    
    public override void Initialize()
    {
      _inputManager = ((HeroPlayer)ComponentOwner).InputManager;
      
      base.Initialize();
    }
  }
}