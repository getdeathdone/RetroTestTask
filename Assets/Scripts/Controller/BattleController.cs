using System;
using DefaultNamespace.Component;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class BattleController : object, IInitialize, IDeinitialize
  {
    public event Action<bool> OnFinishBattle;

    private readonly EnemyController _enemyController;
    private readonly PlayerController _playerController;
    private readonly GameController _gameController;
    private readonly UIManager _uiManager;

    private int _playerKillCounter;

    [Inject]
    public BattleController(EnemyController enemyController, PlayerController playerController, GameController gameController, UIManager uiManager)
    {
      _playerController = playerController;
      _enemyController = enemyController;
      _gameController = gameController;
      _uiManager = uiManager;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }

      InitSubscribe(true);

      _playerKillCounter = 0;
      _uiManager.HUDPanel.UpdatePlayerKill(_playerKillCounter);
      
      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      InitSubscribe(false);
      
      IsInitialized = false;
    }

    private void GetDamage (DamageInfo obj)
    {
      _uiManager.HUDPanel.SpawnBattleInfoCard(obj);
    }

    private void OnDied (DamageInfo damageInfo)
    {
      var hero = ((Health)damageInfo.Receiver).ComponentOwner;

      _uiManager.HUDPanel.SpawnBattleInfoCard(damageInfo, isDead: true);
      
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
      _uiManager.HUDPanel.UpdatePlayerKill(_playerKillCounter);

      if (_playerKillCounter >= GameConstants.Battle.PLAYER_KILL_TO_WIN)
      {
        FinishBattle(true);
      }
    }

    private void FinishBattle (bool win)
    {
      Deinitialize();
      
      _gameController.EndGame();
      OnFinishBattle?.Invoke(win);
    }

    private void InitSubscribe (bool isSubscribe)
    {
      if(isSubscribe)
      {
        _enemyController.OnSpawnEnemy += SpawnSubscribe;
      } else
      {
        _enemyController.OnSpawnEnemy -= SpawnSubscribe;
      }
      
      Subscribe(isSubscribe, _playerController.Player);

      foreach (var heroEnemy in _enemyController.Enemies)
      {
        Subscribe(isSubscribe, heroEnemy);
      }
    }

    private void SpawnSubscribe (HeroBase heroBase)
    {
      Subscribe(true, heroBase);
    }

    private void Subscribe (bool isSubscribe, HeroBase heroBase)
    {
      var health = heroBase.GetAttachedComponent<Health>();
      
      if (isSubscribe)
      {
        health.OnDeath += OnDied;
        health.OnGetDamage += GetDamage;
      } else
      {
        health.OnDeath -= OnDied;
        health.OnGetDamage -= GetDamage;
      }
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}