using System;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class PlayerController : object, IInitialize
  {
    private HeroPlayer _player;
    public HeroPlayer Player => _player;

    private SpawnManager _spawnManager;
    
    [Inject]
    private void Construct (
      SpawnManager spawnManager)
    {
      _spawnManager = spawnManager;
    }

    public void Initialize()
    {
      _player = _spawnManager.SpawnPlayer();
      
      _player.Initialize();
      
      IsInitialized = true;
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}