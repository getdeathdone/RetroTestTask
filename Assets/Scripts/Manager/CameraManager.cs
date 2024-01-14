using DefaultNamespace.Controller;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class CameraManager : MonoBehaviour, IInitialize
  {
    private readonly Vector3 _offset = new Vector3(0, 0, -1 / 4f);

    [SerializeField]
    private float _smoothSpeed = 0.125f;
    [SerializeField]
    private float _cameraHeight = 0.125f;

    private Transform _player;
    private PlayerController _playerController;
    private bool _isInitialized;
    public bool IsInitialized => _isInitialized;
  
    [Inject]
    private void Construct(
      PlayerController playerController)
    {
      _playerController = playerController;
    }
    
    public void Initialize()
    {
      _player = _playerController.Player.transform;
      _isInitialized = true;
    }

    private void LateUpdate()
    {
      if (!IsInitialized)
      {
        return;
      }
    
      Vector3 desiredPosition = _player.position + _offset + Vector3.up * _cameraHeight;
      transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
    
      float targetRotationY = _player.rotation.eulerAngles.y;
      Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
    
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _smoothSpeed);
    }
  }
}