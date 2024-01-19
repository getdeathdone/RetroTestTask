using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class PlayerController : object, IInitialize, IDeinitialize
  {
    private HeroPlayer _player;
    public HeroPlayer Player => _player;

    private SpawnManager _spawnManager;
    private GameController _gameController;
    
    [Inject]
    private void Construct (
      SpawnManager spawnManager,
      GameController gameController)
    {
      _spawnManager = spawnManager;
      _gameController = gameController;
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
      }

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
      
      IsInitialized = false;
    }
    
    private void PlayerEnable(bool value)
    {
      Player.SetActive(value);
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}