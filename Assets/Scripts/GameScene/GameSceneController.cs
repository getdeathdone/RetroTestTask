using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameScene
{
  public class GameSceneController : MonoBehaviour
  {
    private UIManager _uiManager;
    private CameraManager _cameraManager;
    private SpawnHandler _spawnHandler;

    [Inject]
    private void Construct(
      UIManager inputManager,
      CameraManager cameraManager,
      SpawnHandler spawnHandler)
    {
      _uiManager = inputManager;
      _cameraManager = cameraManager;
      _spawnHandler = spawnHandler;
    }

    private void Awake()
    {
      _uiManager.Initialize();
      _spawnHandler.Initialize();
      _cameraManager.Initialize();
    }
  }
}