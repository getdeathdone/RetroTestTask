using UnityEngine;

namespace DefaultNamespace
{
  public class InputPanel : MonoBehaviour
  {
    [SerializeField]
    private ButtonHold _attackButton;
    [SerializeField]
    private ButtonHold _attackUltimateButton;
    [SerializeField]
    private VariableJoystick _variableJoystick;

    public ButtonHold AttackButton => _attackButton;
    public ButtonHold AttackUltimateButton => _attackUltimateButton;
    public VariableJoystick VariableJoystick => _variableJoystick;
  }
}