using System;
using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class BattleController : object, IInitialize, IDeinitialize, IUpdateVisual
  {
    public event Action<bool> OnFinishBattle;
    public event Action<float> OnUpdateVisual;

    private readonly EnemyController _enemyController;
    private readonly PlayerController _playerController;
    private readonly GameController _gameController;

    private int _playerKillCounter;

    [Inject]
    public BattleController(EnemyController enemyController, PlayerController playerController, GameController gameController)
    {
      _playerController = playerController;
      _enemyController = enemyController;
      _gameController = gameController;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }

      Subscribe(true);

      _playerKillCounter = 0;
      
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

    private void OnDied (DamageInfo damageInfo)
    {
      var hero = ((Health)damageInfo.Receiver).ComponentOwner;
      
      if (hero.Side == HeroSide.None)
      {
        Debug.LogWarning("HeroSide.None");
        return;
      }
      
      if (hero.Side == HeroSide.Player)
      {
        FinishBattle(false);
        return;
      }

      if (damageInfo.AttackType == AttackType.Ultimate)
      {
        return;
      }

      _playerKillCounter++;
      OnUpdateVisual?.Invoke(_playerKillCounter);

      if (_playerKillCounter >= GameConstants.Battle.PLAYER_KILL_TO_WIN)
      {
        FinishBattle(true);
      }
    }
    
    private void FinishBattle (bool win)
    {
      Deinitialize();

      OnFinishBattle?.Invoke(win);
      
      _gameController.EndGame();
    }
    
    private void Subscribe (bool value)
    {
      if (value)
      {
        _playerController.Player.OnDeath += OnDied;
        
        foreach (var VARIABLE in _enemyController.Enemies)
        {
          VARIABLE.OnDeath += OnDied;
        }
      } else
      {
        _playerController.Player.OnDeath -= OnDied;
        
        foreach (var VARIABLE in _enemyController.Enemies)
        {
          VARIABLE.OnDeath -= OnDied;
        }
      }
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}