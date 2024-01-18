using System;
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
    private BattleController _battleController;
    private AchievementController _achievementController;

    [Inject]
    private void Construct(
      UIManager inputManager,
      CameraManager cameraManager,
      GameController gameController,
      PlayerController playerController,
      EnemyController enemyController,
      BattleController battleController,
      AchievementController achievementController)
    {
      _uiManager = inputManager;
      _cameraManager = cameraManager;
      _gameController = gameController;
      _playerController = playerController;
      _enemyController = enemyController;
      _battleController = battleController;
      _achievementController = achievementController;
    }

    private void Start()
    {
      _uiManager.Initialize();

      _playerController.Initialize();
      //_enemyController.Initialize();

      _cameraManager.Initialize();
      _battleController.Initialize();
      _achievementController.Initialize();

      _gameController.StartGame();
    }

    private void OnDestroy()
    {
      _battleController.Deinitialize();
      _achievementController.Deinitialize();
    }
  }
}