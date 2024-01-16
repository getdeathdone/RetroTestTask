using System;
using DefaultNamespace.Controller;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class InputManager : object, IInject
  {
    public const int ROTATION_INPUT_SPEED = 200;
    public const float KEYBOARD_SLOWDOWN = 0.2f;
    
    private const bool INVERT_Y_AXIS = true;
    private const bool INVERT_X_AXIS = false;
    private const float LOOK_SENSITIVITY = 1f;
    private const float SMOOTHING_FACTOR = 0.15f;

    private const string MOUSE_AXIS_NAME_VERTICAL = "Mouse Y";
    private const string MOUSE_AXIS_NAME_HORIZONTAL = "Mouse X";
    
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

    public float RotateHorizontalInput => UseRotateKeyboard ? GetRotationKeyboard() : GetMouseOrStickLookAxis(MOUSE_AXIS_NAME_HORIZONTAL, INVERT_X_AXIS);
    public bool UseRotateKeyboard => GetRotationKeyboard() != 0;
    public Type Type => GetType();

    private bool _useRotateKeyboard;
    private float _smoothedTouchInput;

    private int GetRotationKeyboard()
    {
      bool rotateLeftHeld = PlatformManager.IS_MOBILE ? _uiManager.InputPanel.RotateLeftButton.IsButtonHeld : Input.GetKey(KeyCode.Q);
      bool rotateRightHeld = PlatformManager.IS_MOBILE ? _uiManager.InputPanel.RotateRightButton.IsButtonHeld : Input.GetKey(KeyCode.E);

      return rotateLeftHeld && !rotateRightHeld ? -1 : !rotateLeftHeld && rotateRightHeld ? 1 : 0;
    }

    public float GetLookInputsVertical()
    {
      if(!PlatformManager.IS_MOBILE)
      {
        return GetMouseOrStickLookAxis(MOUSE_AXIS_NAME_VERTICAL, INVERT_Y_AXIS);
      } else
      {
        if (Input.touchCount <= 0)
        {
          return 0f;
        }

        Touch touch = Input.GetTouch(0);
          
        if (touch.position.x > Screen.width / 2 && touch.phase == TouchPhase.Moved)
        {
          float touchInput = touch.deltaPosition.x * Time.deltaTime;
          _smoothedTouchInput = Mathf.Lerp(_smoothedTouchInput, touchInput, SMOOTHING_FACTOR);

          return _smoothedTouchInput;
        }

        return 0f;
      }
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