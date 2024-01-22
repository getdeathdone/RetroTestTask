using System;
using DefaultNamespace.Component;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class PlayerController : object, IInitialize, IDeinitialize
  {
    public event Action<HeroPlayer> OnSpawnPlayer;
    
    private HeroPlayer _player;
    public HeroPlayer Player => _player;

    private SpawnManager _spawnManager;
    private GameController _gameController;
    private UIManager _uiManager;
    
    [Inject]
    private void Construct (
      SpawnManager spawnManager,
      GameController gameController,
      UIManager uiManager)
    {
      _spawnManager = spawnManager;
      _gameController = gameController;
      _uiManager = uiManager;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }
      
      _gameController.OnPause += PlayerEnable;
      
      if (_player == null)
      {
        _player = _spawnManager.SpawnPlayer();
        OnSpawnPlayer?.Invoke(_player);
      }

      _player.GetAttachedComponent<Health>().OnUpdateVisual += _uiManager.HUDPanel.SetHealth;
      
      _player.Initialize();
      
      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      _gameController.OnPause -= PlayerEnable;
      
      _player.GetAttachedComponent<Health>().OnUpdateVisual -= _uiManager.HUDPanel.SetHealth;
      
      IsInitialized = false;
    }
    
    private void PlayerEnable(bool value)
    {
      Player.SetActive(!value);
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}