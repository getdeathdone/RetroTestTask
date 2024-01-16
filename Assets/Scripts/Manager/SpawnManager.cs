using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.ScriptableObjects;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class SpawnManager : MonoBehaviour
  {
    [SerializeField]
    private HeroTypeDictionary _heroPrefabs = new HeroTypeDictionary();
    [SerializeField]
    private HeroDataDictionary _heroData = new HeroDataDictionary();

    private readonly List<HeroBase> _heroBases = new List<HeroBase>();

    private AreaManager _areaManager;
    private InputManager _inputManager;

    private Vector3 GenerateRandomPositionInAreaRadius => GenerateRandomPositionInCircle(_areaManager.Radius);

    [Inject]
    private void Construct (
      AreaManager areaManager, 
      InputManager inputManager)
    {
      _areaManager = areaManager;
      _inputManager = inputManager;
    }

    public HeroPlayer SpawnPlayer()
    {
      var player = SpawnHero(HeroType.Player, GenerateRandomPositionInAreaRadius);
      player.name = $"{player.Type}";

      return (HeroPlayer)player;
    }

    public HeroEnemy SpawnEnemy (HeroType enemyType)
    {
      if (enemyType == HeroType.Player)
      {
        return null;
      }
      
      return (HeroEnemy)SpawnHero(enemyType, GenerateRandomPositionInAreaRadius);
    }

    private HeroBase SpawnHero (HeroType heroType, Vector3 position)
    {
      if (heroType == HeroType.None)
      {
        Debug.LogWarning("Spawn HeroType.None");
        return null;
      }
      
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
        
        case HeroType.EnemyRed:
        case HeroType.EnemyBlue:
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
        
        case HeroType.EnemyRed:
        case HeroType.EnemyBlue:
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

    private Vector3 GenerateRandomPositionInCircle(float radius)
    {
      float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
      float distance = UnityEngine.Random.Range(0f, radius);
      
      float spawnX = distance * Mathf.Cos(angle);
      float spawnZ = distance * Mathf.Sin(angle);

      return new Vector3(spawnX, 0f, spawnZ);
    }

    [Serializable]
    public class HeroTypeDictionary : SerializableDictionaryBase<HeroType, HeroBase>
    {}

    [Serializable]
    public class HeroDataDictionary : SerializableDictionaryBase<HeroType, HeroData>
    {}
  }
}