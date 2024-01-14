using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameScene
{
  public class GameSceneController : MonoBehaviour
  {
    private UIManager _uiManager;
    private CameraManager _cameraManager;
    private SpawnManager _spawnManager;

    [Inject]
    private void Construct(
      UIManager inputManager,
      CameraManager cameraManager,
      SpawnManager spawnManager)
    {
      _uiManager = inputManager;
      _cameraManager = cameraManager;
      _spawnManager = spawnManager;
    }

    private void Awake()
    {
      _uiManager.Initialize();
      _spawnManager.Initialize();
      _cameraManager.Initialize();
    }
  }
}