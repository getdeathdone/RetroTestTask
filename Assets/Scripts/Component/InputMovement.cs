using DefaultNamespace.Hero;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class InputMovement : Movement
  {
    public const float KEYBOARD_SLOWDOWN = 0.2f;
    
    private InputManager _inputManager;
    protected override Vector3 MovementDirection => _transform.TransformDirection(_inputManager.Direction);
    protected override float Rotate => _inputManager.RotateHorizontalInput;
    protected override bool UseSlowdown => _inputManager.UseRotateKeyboard;

    public override void Initialize()
    {
      _inputManager = ((HeroPlayer)ComponentOwner).InputManager;
      
      base.Initialize();
    }
  }
}