using DefaultNamespace.Hero;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Component
{
  public class InputMovement : Movement
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
  }
}