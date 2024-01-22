using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
  public class InputPanel : MonoBehaviour
  {
    [SerializeField]
    private Button _attackButton;
    [SerializeField]
    private Button _attackUltimateButton;
    [SerializeField]
    private VariableJoystick _variableJoystick;

    private bool _attackUltimateButtonInvoke;
    private bool _attackButtonInvoke;

    public bool AttackButton => _attackButtonInvoke;
    public bool AttackUltimateButton => _attackUltimateButtonInvoke;
    public VariableJoystick VariableJoystick => _variableJoystick;

    private void Awake()
    {
      _attackButton.onClick.AddListener(OnAttackButtonClick);
      _attackUltimateButton.onClick.AddListener(OnAttackUltimateButtonClick);
    }

    private void OnAttackButtonClick()
    {
      _attackButtonInvoke = true;
      StartCoroutine(ResetButtonInvoke());
    }

    private void OnAttackUltimateButtonClick()
    {
      _attackUltimateButtonInvoke = true;
      StartCoroutine(ResetButtonInvoke());
    }

    private IEnumerator ResetButtonInvoke()
    {
      yield return new WaitForEndOfFrame();
      
      _attackButtonInvoke = false;
      _attackUltimateButtonInvoke = false;
    }
  }
}