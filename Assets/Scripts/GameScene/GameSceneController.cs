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
    private GameController _gameController;
    private PlayerController _playerController;
    private EnemyController _enemyController;

    [Inject]
    private void Construct(
      UIManager inputManager,
      CameraManager cameraManager,
      GameController gameController,
      PlayerController playerController,
      EnemyController enemyController)
    {
      _uiManager = inputManager;
      _cameraManager = cameraManager;
      _gameController = gameController;
      _playerController = playerController;
      _enemyController = enemyController;
    }

    private void Start()
    {
      _uiManager.Initialize();
      _cameraManager.Initialize();

      _playerController.Initialize();
      _enemyController.Initialize();
      _gameController.StartGame();
    }
  }
}