using System;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class BattleController : object, IInitialize, IDeinitialize, IUpdateVisual
  {
    public const int PLAYER_KILL_TO_WIN = 20;

    public event Action<bool> OnFinishBattle;
    public event Action<float> OnUpdateVisual;

    private readonly EnemyController _enemyController;
    private readonly BattleStatsPanel _battleStatsPanel;
    private readonly PlayerController _playerController;

    private int _playerKillCounter;
    
    [Inject]
    public BattleController(EnemyController enemyController, PlayerController playerController)
    {
      _playerController = playerController;
      _enemyController = enemyController;
    }

    public void Initialize()
    {
      _playerController.Player.OnDeath += OnDied;
      
      foreach (var VARIABLE in _enemyController.Enemies)
      {
        VARIABLE.OnDeath += OnDied;
      }
      
      
      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      _playerController.Player.OnDeath -= OnDied;
      
      foreach (var VARIABLE in _enemyController.Enemies)
      {
        VARIABLE.OnDeath -= OnDied;
      }
      
      IsInitialized = false;
    }

    private void OnDied (HeroBase character)
    {
      if (character.Side == HeroSide.None)
      {
        Debug.LogWarning("HeroSide.None");
        return;
      }
      
      if (character.Side == HeroSide.Player)
      {
        FinishBattle(false);
        return;
      }

      _playerKillCounter++;
      OnUpdateVisual?.Invoke(_playerKillCounter);

      if (_playerKillCounter >= PLAYER_KILL_TO_WIN)
      {
        FinishBattle(true);
      }
    }
    

    private void FinishBattle (bool win)
    {
      Deinitialize();

      OnFinishBattle?.Invoke(win);
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}