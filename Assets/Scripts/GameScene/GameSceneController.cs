using DefaultNamespace.Controller;
using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameScene
{
  public class GameSceneController : MonoBehaviour
  {
    private UIManager _uiManager;
    private GameController _gameController;
    private PlayerController _playerController;
    private EnemyController _enemyController;
    private AreaController _areaController;
    private BattleController _battleController;
    private AchievementController _achievementController;

    [Inject]
    private void Construct(
      UIManager inputManager,
      GameController gameController,
      PlayerController playerController,
      AreaController areaController,
      EnemyController enemyController,
      BattleController battleController,
      AchievementController achievementController)
    {
      _uiManager = inputManager;
      _gameController = gameController;
      _playerController = playerController;
      _enemyController = enemyController;
      _battleController = battleController;
      _achievementController = achievementController;
      _areaController = areaController;
    }

    private void Awake()
    {
      _gameController.OnRestart += Restart;
      _areaController.Initialize();
      
      _uiManager.Initialize();
    }

    private void Start()
    {
      InitializeGame();
      _uiManager.PausePanel.OpenCloseMenu();
    }

    private void OnDestroy()
    {
      _gameController.OnRestart -= Restart;
      _areaController.Deinitialize();
      
      Deinitialize();
    }

    private void InitializeGame()
    {
      _playerController.Initialize();
      _enemyController.Initialize();

      _battleController.Initialize();
      _achievementController.Initialize();
    }

    private void Deinitialize()
    {
      _battleController.Deinitialize();
      _achievementController.Deinitialize();
      
      _playerController.Deinitialize();
      _enemyController.Deinitialize();
    }

    private void Restart()
    {
      Deinitialize();
      InitializeGame();
    }
  }
}