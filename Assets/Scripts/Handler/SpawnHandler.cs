using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Handler;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class SpawnHandler : MonoBehaviour, IInitialize
  {
    [SerializeField]
    private HeroBase _heroBase;
    [SerializeField]
    private Transform _spawnPlayerPos;
    
    private readonly List<HeroBase> _heroBases = new List<HeroBase>();
    private AreaManager _areaManager;
    private InputHandler _inputHandler;


    [Inject]
    private void Construct(
      AreaManager areaManager, 
      InputHandler inputHandler)
    {
      _areaManager = areaManager;
      _inputHandler = inputHandler;
    }
    
    
    private void SpawnPlayer (Vector3 position)
    {
      var hero = Instantiate(_heroBase, position, _heroBase.transform.rotation);
      hero.name = GameConstance.PLAYER_NAME;

      hero.SetInject(_areaManager, _inputHandler);
      hero.AddComponent<MovementComponentBase>();
      
      _heroBases.Add(hero);
    }

    public void Initialize()
    {
      SpawnPlayer(_spawnPlayerPos.position);

      foreach (var VARIABLE in _heroBases)
      {
        VARIABLE.Initialize();
      }
      
      IsInitialized = true;
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}