using System;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class BattleController : object, IInitialize, IDeinitialize
  {
    public event Action<bool> OnFinishBattle;
    
    private readonly GameController _gameController;
    private readonly UIManager _uiManager;

    private int _playerKillCounter;

    [Inject]
    public BattleController(GameController gameController, UIManager uiManager)
    {
      _gameController = gameController;
      _uiManager = uiManager;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }

      Subscribe(true);

      _playerKillCounter = 0;
      //_uiManager.HUDPanel.UpdatePlayerKill(_playerKillCounter);
      
      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      Subscribe(false);
      
      IsInitialized = false;
    }

    private void OnDied ()
    {
      /*if (hero.Side == HeroSide.Player)
      {
        FinishBattle(false);
        return;
      }
      
      _playerKillCounter++;
      _uiManager.HUDPanel.UpdatePlayerKill(_playerKillCounter);

      if (_playerKillCounter >= GameConstants.Battle.PLAYER_KILL_TO_WIN)
      {
        FinishBattle(true);
      }*/
    }

    private void FinishBattle (bool win)
    {
      Deinitialize();
      
      _gameController.EndGame();
      OnFinishBattle?.Invoke(win);
    }
    
    private void Subscribe (bool isSubscribe)
    {
      
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}