using System;
using DefaultNamespace.Controller;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class InputManager : object, IInject
  {
    private const bool INVERT_Y_AXIS = true;
    private const bool INVERT_X_AXIS = false;
    private const float LOOK_SENSITIVITY = 1f;

    private readonly UIManager _uiManager;
    private readonly GameController _gameController;

    [Inject]
    public InputManager (UIManager uiManager, GameController gameController)
    {
      _uiManager = uiManager;
      _gameController = gameController;
    }

    private VariableJoystick VariableJoystick => _uiManager.InputPanel.VariableJoystick;
    private Vector3 DirectionJoystick => Vector3.forward * VariableJoystick.Vertical + Vector3.right * VariableJoystick.Horizontal;
    private Vector3 DirectionKeyboard => Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
    public Vector3 Direction => PlatformManager.IS_MOBILE ? DirectionJoystick : DirectionKeyboard;

    public float RotateHorizontalInput => UseRotateKeyboard ? GetRotationKeyboard() : GetLookInputsHorizontal();
    public bool UseRotateKeyboard => GetRotationKeyboard() != 0;
    public Type Type => GetType();

    private bool _useRotateKeyboard;

    private int GetRotationKeyboard()
    {
      bool rotateLeftHeld = PlatformManager.IS_MOBILE ? _uiManager.InputPanel.RotateLeftButton : Input.GetKey(KeyCode.Q);
      bool rotateRightHeld = PlatformManager.IS_MOBILE ? _uiManager.InputPanel.RotateRightButton : Input.GetKey(KeyCode.E);

      return rotateLeftHeld && !rotateRightHeld ? -1 : !rotateLeftHeld && rotateRightHeld ? 1 : 0;
    }

    public float GetLookInputsVertical()
    {
      return GetMouseOrStickLookAxis(GameConstants.k_MouseAxisNameVertical, INVERT_Y_AXIS);
    }

    private float GetLookInputsHorizontal()
    {
      return GetMouseOrStickLookAxis(GameConstants.k_MouseAxisNameHorizontal, INVERT_X_AXIS);
    }

    private float GetMouseOrStickLookAxis (string mouseInputName, bool invertAxis)
    {
      if (!CanProcessInput())
      {
        return 0f;
      }

      float i = Input.GetAxisRaw(mouseInputName);
        
      if (invertAxis)
      {
        i *= -1f;
      }
        
      i *= LOOK_SENSITIVITY;
      i *= 0.01f;

      return i;
    }

    private bool CanProcessInput()
    {
      return Cursor.lockState == CursorLockMode.Locked && !_gameController.IsPaused;
    }
  }
}