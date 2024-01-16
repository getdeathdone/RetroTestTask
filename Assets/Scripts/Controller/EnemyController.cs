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
  public class EnemyController : object, IInitialize
  {
    private const int BLUE_ENEMY_RATIO = 1;
    private const int RED_ENEMY_RATIO = 4;
    
    private const int MAX_ENEMY_COUNT = 30;
    private const int START_ENEMY_COUNT = 5;
    
    private const float INITIAL_SPAWN_INTERVAL = 30f;
    private const float MIN_SPAWN_INTERVAL = 6.0f;
    private const float SPAWN_INTERVAL_DECREMENT = 2.0f;
    
    public event Action<HeroEnemy> OnSpawnEnemy;

    private readonly List<HeroEnemy> _enemies = new List<HeroEnemy>();
    public List<HeroEnemy> Enemies => _enemies;

    private readonly CoroutineHandler _coroutineHandler;
    private readonly SpawnManager _spawnManager;
    private float _currentSpawnInterval;

    [Inject]
    private EnemyController (
      SpawnManager spawnManager, 
      CoroutineHandler coroutineHandler)
    {
      _spawnManager = spawnManager;
      _coroutineHandler = coroutineHandler;
    }

    public void Initialize()
    {
      GenerateEnemy(START_ENEMY_COUNT);

      foreach (var VARIABLE in _enemies)
      {
        VARIABLE.Initialize();
      }

      _currentSpawnInterval = INITIAL_SPAWN_INTERVAL;
      _coroutineHandler.StartRoutine(SpawnEnemyRoutine());
      IsInitialized = true;
    }

    private void GenerateEnemy()
    {
      var type = DetermineEnemyType();
      SpawnEnemy(type);

      HeroType DetermineEnemyType()
      {
        int totalBlueEnemies = Enemies.Count(enemy => enemy.Type == HeroType.EnemyBlue);
        int totalRedEnemies = Enemies.Count(enemy => enemy.Type == HeroType.EnemyRed);

        return totalBlueEnemies < totalRedEnemies * RED_ENEMY_RATIO ? HeroType.EnemyBlue : HeroType.EnemyRed;
      }
    }

    private void GenerateEnemy (int count)
    {
      for (int index = 0; index < count; index++)
      {
        const int TOTAL_ENEMY_RATIO = BLUE_ENEMY_RATIO + RED_ENEMY_RATIO;

        HeroType enemyType = (index % TOTAL_ENEMY_RATIO == 0) ? HeroType.EnemyBlue : HeroType.EnemyRed;

        SpawnEnemy(enemyType);
      }
    }

    private void SpawnEnemy (HeroType enemyType)
    {
      if (Enemies.Count == MAX_ENEMY_COUNT)
      {
        Debug.Log("MAX_ENEMY_COUNT");
        return;
      }

      var enemy = _spawnManager.SpawnEnemy(enemyType);
      _enemies.Add(enemy);
      int index = _enemies.IndexOf(enemy);

      enemy.name = $"{GameConstants.ENEMY_NAME} {index} {enemyType}";
      
      OnSpawnEnemy?.Invoke(enemy);
    }
    
    private IEnumerator SpawnEnemyRoutine()
    {
      while (true)
      {
        yield return new WaitForSeconds(_currentSpawnInterval);
        
        _currentSpawnInterval = Mathf.Max(MIN_SPAWN_INTERVAL, _currentSpawnInterval - SPAWN_INTERVAL_DECREMENT);
        
        GenerateEnemy();
      }
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}