using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Component.AI;
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
    private Transform _heroParent;
    
    [SerializeField]
    private HeroTypeDictionary _heroPrefabs = new HeroTypeDictionary();
    [SerializeField]
    private HeroDataDictionary _heroData = new HeroDataDictionary();

    private readonly List<HeroBase> _heroBases = new List<HeroBase>();
    
    private InputManager _inputManager;

    [Inject]
    private void Construct (
      InputManager inputManager)
    {
      _inputManager = inputManager;
    }

    public HeroPlayer SpawnPlayer()
    {
      var player = SpawnHero(HeroType.Player);
      player.name = $"{player.Type}";

      return (HeroPlayer)player;
    }

    public HeroEnemy SpawnEnemy (HeroType enemyType)
    {
      if (enemyType == HeroType.Player)
      {
        return null;
      }
      
      return (HeroEnemy)SpawnHero(enemyType);
    }

    private HeroBase SpawnHero (HeroType heroType)
    {
      if (heroType == HeroType.None)
      {
        Debug.LogWarning("Spawn HeroType.None");
        return null;
      }
      
      var heroBase = _heroPrefabs[heroType];
      var hero = Instantiate(heroBase, _heroParent.transform);

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
      heroBase.AddComponent<Health>();

      if (heroBase.Side == HeroSide.Player && heroType == HeroType.Player)
      {
        BuildPlayer();
      }else if (heroBase.Side == HeroSide.Enemy)
      {
        if (heroType == HeroType.EnemyBlue)
        {
          BuildEnemyBlue();
        }else if (heroType == HeroType.EnemyRed)
        {
          BuildEnemyRed();
        }
      }

      void BuildPlayer()
      {
        heroBase.AddComponent<Attack>();
        heroBase.AddComponent<Movement>();
      }
      
      void BuildEnemyBlue()
      {
        heroBase.AddComponent<AttackRange>();
        heroBase.AddComponent<Navigation>();
      }
      
      void BuildEnemyRed()
      {
        heroBase.AddComponent<FlyKiller>();
      }
    }

    [Serializable]
    public class HeroTypeDictionary : SerializableDictionaryBase<HeroType, HeroBase>
    {}

    [Serializable]
    public class HeroDataDictionary : SerializableDictionaryBase<HeroType, HeroData>
    {}
  }
}