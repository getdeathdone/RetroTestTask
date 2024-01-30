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
    private BattleController _battleController;

    [Inject]
    private void Construct(
      UIManager inputManager,
      GameController gameController,
      BattleController battleController)
    {
      _uiManager = inputManager;
      _gameController = gameController;
      _battleController = battleController;
    }

    private void Awake()
    {
      Application.targetFrameRate = 60;
      
      _gameController.OnRestart += Restart;

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

      Deinitialize();
    }

    private void InitializeGame()
    {
      _battleController.Initialize();
    }

    private void Deinitialize()
    {
      _battleController.Deinitialize();
    }

    private void Restart()
    {
      Deinitialize();
      InitializeGame();
    }
  }
}