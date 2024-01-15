using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Controller;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.ScriptableObjects;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class SpawnManager : MonoBehaviour, IInitialize
  {
    [SerializeField]
    private Transform _spawnPlayerPos;
    [SerializeField]
    private List<Transform> _spawnEnemyPos;

    [SerializeField]
    private HeroTypeDictionary _heroPrefabs = new HeroTypeDictionary();
    [SerializeField]
    private HeroDataDictionary _heroData = new HeroDataDictionary();

    private readonly List<HeroBase> _heroBases = new List<HeroBase>();

    private AreaManager _areaManager;
    private InputManager _inputManager;
    private PlayerController _playerController;
    private EnemyController _enemyController;

    [Inject]
    private void Construct (
      AreaManager areaManager, 
      InputManager inputManager, 
      PlayerController playerController,
      EnemyController enemyController)
    {
      _areaManager = areaManager;
      _inputManager = inputManager;
      _playerController = playerController;
      _enemyController = enemyController;
    }

    public void Initialize()
    {
      SpawnPlayer();
      SpawnEnemy();

      foreach (var VARIABLE in _heroBases)
      {
        VARIABLE.Initialize();
      }

      IsInitialized = true;
    }

    private void SpawnPlayer()
    {
      var player = SpawnHero(HeroType.Player, _spawnPlayerPos.position);
      _playerController.SetPlayer((HeroPlayer)player);
      
      player.name = GameConstants.PLAYER_NAME;
    }

    private void SpawnEnemy()
    {
      for (int index = 0; index < _spawnEnemyPos.Count; index++)
      {
        Transform point = _spawnEnemyPos[index];
        var enemy = SpawnHero(HeroType.Enemy, point.position);
        _enemyController.AddEnemy((HeroEnemy)enemy);

        enemy.name = $"{GameConstants.ENEMY_NAME} {index}";
      }
    }

    private HeroBase SpawnHero (HeroType heroType, Vector3 position)
    {
      var heroBase = _heroPrefabs[heroType];
      var hero = Instantiate(heroBase, position, heroBase.transform.rotation);

      List<IInject> injects = AdditionInject(heroType);
      hero.SetInject(injects);

      AdditionComponent(heroType, hero);

      _heroBases.Add(hero);

      return hero;
    }

    List<IInject> AdditionInject (HeroType heroType)
    {
      List<IInject> injects = new List<IInject>
      {
        _areaManager,
        _heroData[heroType]
      };

      switch (heroType)
      {
        case HeroType.Player:
          injects.Add(_inputManager);
          break;
        case HeroType.Enemy:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(heroType), heroType, null);
      }

      return injects;
    }

    private void AdditionComponent (HeroType heroType, HeroBase heroBase)
    {
      switch (heroType)
      {
        case HeroType.Player:
          BuildPlayer();
          break;
        case HeroType.Enemy:
          BuildEnemy();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(heroType), heroType, null);
      }

      void BuildPlayer()
      {
        heroBase.AddComponent<InputMovement>();
      }

      void BuildEnemy()
      {}
    }

    public bool IsInitialized
    {
      get;
      private set;
    }

    [Serializable]
    public class HeroTypeDictionary : SerializableDictionaryBase<HeroType, HeroBase>
    {}

    [Serializable]
    public class HeroDataDictionary : SerializableDictionaryBase<HeroType, HeroData>
    {}
  }
}