using DefaultNamespace.Controller;
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
    private GameController _gameController;

    [Inject]
    private void Construct(
      UIManager inputManager,
      CameraManager cameraManager,
      SpawnManager spawnManager,
      GameController gameController)
    {
      _uiManager = inputManager;
      _cameraManager = cameraManager;
      _spawnManager = spawnManager;
      _gameController = gameController;
    }

    private void Awake()
    {
      _uiManager.Initialize();
      _spawnManager.Initialize();
      _cameraManager.Initialize();

      _gameController.StartGame();
    }
  }
}