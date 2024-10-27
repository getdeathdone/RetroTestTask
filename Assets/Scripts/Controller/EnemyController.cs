using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Handler;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class EnemyController : object, IInitialize, IDeinitialize
  {
    public event Action<HeroEnemy> OnSpawnEnemy;

    private readonly List<HeroEnemy> _enemies = new List<HeroEnemy>();
    public List<HeroEnemy> Enemies => _enemies;

    private readonly CoroutineHandler _coroutineHandler;
    private readonly SpawnManager _spawnManager;
    private readonly GameController _gameController;
    private float _currentSpawnInterval;
    private Coroutine _coroutine;

    [Inject]
    private EnemyController (
      SpawnManager spawnManager, 
      CoroutineHandler coroutineHandler,
      GameController gameController)
    {
      _gameController = gameController;
      _spawnManager = spawnManager;
      _coroutineHandler = coroutineHandler;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }
      
      _gameController.OnPause += EnemyEnable;
      GenerateEnemy(GameConstants.Enemy.START_ENEMY_COUNT);

      _currentSpawnInterval = GameConstants.Enemy.INITIAL_SPAWN_INTERVAL;
      _coroutine = _coroutineHandler.StartRoutine(SpawnEnemyRoutine());
      
      IsInitialized = true;
    }
    
    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      _gameController.OnPause -= EnemyEnable;

      if (_coroutineHandler != null)
      {
        _coroutineHandler.StopRoutine(_coroutine);
      }

      for (int index = _enemies.Count - 1; index >= 0; index--)
      {
        HeroEnemy VARIABLE = _enemies[index];

        if (VARIABLE != null)
        {
          VARIABLE.DestroyEnemy();
        }

        Enemies.RemoveAt(index);
      }
      
      Enemies.Clear();
      
      IsInitialized = false;
    }

    private void EnemyEnable(bool value)
    {
      foreach (var VARIABLE in Enemies)
      {
        VARIABLE.SetActive(!value);
      }
    }

    private void GenerateEnemy()
    {
      var type = DetermineEnemyType();
      SpawnEnemy(type);

      HeroType DetermineEnemyType()
      {
        int totalBlueEnemies = Enemies.Count(enemy => enemy.Type == HeroType.EnemyBlue);
        int totalRedEnemies = Enemies.Count(enemy => enemy.Type == HeroType.EnemyRed);

        return totalBlueEnemies < totalRedEnemies * GameConstants.Enemy.RED_ENEMY_RATIO ? HeroType.EnemyBlue : HeroType.EnemyRed;
      }
    }

    private void GenerateEnemy (int count)
    {
      for (int index = 0; index < count; index++)
      {
        const int TOTAL_ENEMY_RATIO = GameConstants.Enemy.BLUE_ENEMY_RATIO + GameConstants.Enemy.RED_ENEMY_RATIO;

        HeroType enemyType = (index % TOTAL_ENEMY_RATIO == 0) ? HeroType.EnemyBlue : HeroType.EnemyRed;

        SpawnEnemy(enemyType);
      }
    }

    private void SpawnEnemy (HeroType enemyType)
    {
      if (Enemies.FindAll(x => x.gameObject.activeSelf).Count == GameConstants.Enemy.MAX_ENEMY_COUNT)
      {
        Debug.Log("MAX_ENEMY_COUNT");
        return;
      }

      var enemy = _spawnManager.Spawn(enemyType) as HeroEnemy;
      _enemies.Add(enemy);
      int index = _enemies.IndexOf(enemy);

      enemy.name = $"{enemyType} {index}";
      enemy.Initialize();

      if (!_gameController.IsPaused)
      {
        enemy.SetActive(true);
      }
      
      OnSpawnEnemy?.Invoke(enemy);
    }
    
    private IEnumerator SpawnEnemyRoutine()
    {
      while (true)
      {
        while (!_gameController.IsPaused)
        {
          yield return new WaitForSeconds(_currentSpawnInterval);
        
          _currentSpawnInterval = Mathf.Max(GameConstants.Enemy.MIN_SPAWN_INTERVAL, _currentSpawnInterval - GameConstants.Enemy.SPAWN_INTERVAL_DECREMENT);
        
          GenerateEnemy();
        }

        yield return null;
      }
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}