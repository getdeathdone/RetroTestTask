using UnityEngine;

namespace DefaultNamespace
{
  public class InputPanel : MonoBehaviour
  {
    [SerializeField]
    private VariableJoystick _variableJoystick;
    [SerializeField]
    private ButtonHold _rotateLeftButton;
    [SerializeField]
    private ButtonHold _rotateRightButton;

    public VariableJoystick VariableJoystick => _variableJoystick;
    public ButtonHold RotateLeftButton => _rotateLeftButton;
    public ButtonHold RotateRightButton => _rotateRightButton;
  }
}