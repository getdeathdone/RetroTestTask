using System;
using DefaultNamespace.Controller;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class InputManager : object, IInject
  {
    private const int ROTATION_INPUT_SPEED = 200;
    private const int ROTATION_INPUT_MOBILE_SPEED = 20;
    
    private const bool INVERT_Y_AXIS = true;
    private const bool INVERT_X_AXIS = false;
    private const float LOOK_SENSITIVITY = 1f;
    private const float SMOOTHING_FACTOR = 0.15f;

    private const string MOUSE_AXIS_NAME_VERTICAL = "Mouse Y";
    private const string MOUSE_AXIS_NAME_HORIZONTAL = "Mouse X";
    
    private readonly UIManager _uiManager;
    private readonly GameController _gameController;
    
    public Vector3 Direction => GetDirection();
    public float RotateVertical => GetRotateVertical();
    public float RotateHorizontal => GetRotateHorizontal();

    public bool Attack => PlatformManager.IS_MOBILE ? _uiManager.InputPanel.AttackButton : Input.GetKeyDown(KeyCode.Space);
    public bool Pause => Input.GetKeyDown(KeyCode.Escape);
    public bool UltimateAttack => PlatformManager.IS_MOBILE ? _uiManager.InputPanel.AttackUltimateButton : Input.GetKeyDown(KeyCode.LeftAlt);
    
    public Type Type => GetType();

    private bool _useRotateKeyboard;
    private float _smoothedTouchInputY;
    private float _smoothedTouchInputX;

    [Inject]
    public InputManager (UIManager uiManager, GameController gameController)
    {
      _uiManager = uiManager;
      _gameController = gameController;
    }

    private Vector3 GetDirection()
    {
      Vector3 direction;

      if (PlatformManager.IS_MOBILE)
      {
        VariableJoystick variableJoystick = _uiManager.InputPanel.VariableJoystick;

        direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
      } else
      {
        direction = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
      }

      return direction;
    }

    private float GetRotateHorizontal()
    {
      if(!PlatformManager.IS_MOBILE)
      {
        return GetMouseAxis(MOUSE_AXIS_NAME_HORIZONTAL, INVERT_X_AXIS) * ROTATION_INPUT_SPEED;
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
          _smoothedTouchInputX = Mathf.Lerp(_smoothedTouchInputX, touchInput, SMOOTHING_FACTOR);
          int invert = INVERT_X_AXIS ? -1 : 1;
          return invert * (_smoothedTouchInputX * ROTATION_INPUT_MOBILE_SPEED);
        }

        return 0f;
      }
    }

    private float GetRotateVertical()
    {
      if(!PlatformManager.IS_MOBILE)
      {
        return GetMouseAxis(MOUSE_AXIS_NAME_VERTICAL, INVERT_Y_AXIS) * ROTATION_INPUT_SPEED;
      } else
      {
        if (Input.touchCount <= 0)
        {
          return 0f;
        }

        Touch touch = Input.GetTouch(0);
          
        if (touch.position.x > Screen.width / 2 && touch.phase == TouchPhase.Moved)
        {
          float touchInput = touch.deltaPosition.y * Time.deltaTime;
          _smoothedTouchInputY = Mathf.Lerp(_smoothedTouchInputY, touchInput, SMOOTHING_FACTOR);
          int invert = INVERT_Y_AXIS ? -1 : 1;
          return invert * (_smoothedTouchInputY * ROTATION_INPUT_MOBILE_SPEED);
        }

        return 0f;
      }
    }

    private float GetMouseAxis (string mouseInputName, bool invertAxis)
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
      
      bool CanProcessInput()
      {
        return Cursor.lockState == CursorLockMode.Locked && !_gameController.IsPaused;
      }
    }
  }
}