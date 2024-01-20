using UnityEngine;

namespace DefaultNamespace
{
  public class InputPanel : MonoBehaviour
  {
    [SerializeField]
    private VariableJoystick _variableJoystick;

    public VariableJoystick VariableJoystick => _variableJoystick;
  }
}