using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Handler
{
  public class InputHandler
  {
    private readonly UIManager _uiManager;
    
    private VariableJoystick VariableJoystick => _uiManager.InputPanel.VariableJoystick;
    private ButtonHold RotateLeftButton => _uiManager.InputPanel.RotateLeftButton;
    private ButtonHold RotateRightButton => _uiManager.InputPanel.RotateRightButton;

    [Inject]
    public InputHandler(UIManager uiManager)
    {
      _uiManager = uiManager;
    }

    private Vector3 DirectionJoystick => Vector3.forward * VariableJoystick.Vertical + Vector3.right * VariableJoystick.Horizontal;
    private Vector3 DirectionKeyboard => Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
    public Vector3 Direction => PlatformManager.IS_MOBILE ? DirectionJoystick : DirectionKeyboard;
    public bool RotateLeftHeld => PlatformManager.IS_MOBILE ? RotateLeftButton.IsButtonHeld : Input.GetKey(KeyCode.Q);
    public bool RotateRightHeld => PlatformManager.IS_MOBILE ? RotateRightButton.IsButtonHeld : Input.GetKey(KeyCode.E);
  }
}