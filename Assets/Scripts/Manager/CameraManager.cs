using System;
using DefaultNamespace.Controller;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class CameraManager : MonoBehaviour
  {
    private readonly Vector3 _offset = new Vector3(0, 0.2f, 0);

    [SerializeField]
    private float _smoothSpeed = 0.125f;

    private Transform _playerTransform;
    private InputManager _inputManager;
    private PlayerController _playerController;
    private bool _isInitialized;
    private bool _isCameraToPlayer;
    private float _mCameraVerticalAngle;
    public bool IsInitialized => _isInitialized;
  
    [Inject]
    private void Construct(
      InputManager inputManager,
      PlayerController playerController)
    {
      _inputManager = inputManager;
      _playerController = playerController;
    }
    
    public void Awake()
    {
      _playerController.OnSpawnPlayer += SpawnPlayer;
    }

    private void OnDestroy()
    {
      _playerController.OnSpawnPlayer -= SpawnPlayer;
    }

    private void Update()
    {
      if (!_isCameraToPlayer)
      {
        return;
      }

      if (!_playerController.Player.IsActive)
      {
        return;
      }
      
      _mCameraVerticalAngle += _inputManager.RotateVertical;
      
      _mCameraVerticalAngle = Mathf.Clamp(_mCameraVerticalAngle, -89f, 89f);
      
      transform.localEulerAngles = new Vector3(_mCameraVerticalAngle, 0, 0);
    }

    private void LateUpdate()
    {
      if (_isCameraToPlayer)
      {
        return;
      }
      
      if (!_playerController.Player.IsActive)
      {
        return;
      }
    
      Vector3 desiredPosition = _playerTransform.position + _offset;
      transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
    
      float targetRotationY = _playerTransform.rotation.eulerAngles.y;
      Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
    
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _smoothSpeed);

      if (transform.position == desiredPosition)
      {
        transform.SetParent(_playerTransform);
        
        _isCameraToPlayer = true;
      }
    }

    private void SpawnPlayer (HeroPlayer obj)
    {
      _playerTransform = _playerController.Player.transform;
    }
  }
}