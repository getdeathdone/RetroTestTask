using DefaultNamespace.Controller;
using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameScene
{
  public class GameSceneInstaller : MonoInstaller
  {
    [SerializeField]
    private AreaController _areaController;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private CameraManager _cameraManager;

    public override void InstallBindings()
    {
      Container.Bind<InputManager>().AsSingle().NonLazy();
      Container.Bind<GameController>().AsSingle().NonLazy();
      Container.Bind<EnemyController>().AsSingle().NonLazy();
      Container.Bind<PlayerController>().AsSingle().NonLazy();
      Container.Bind<BattleController>().AsSingle().NonLazy();
      Container.Bind<AchievementController>().AsSingle().NonLazy();

      Container.Bind<CameraManager>().FromInstance(_cameraManager).AsSingle().NonLazy();
      Container.Bind<AreaController>().FromInstance(_areaController).AsSingle().NonLazy();
      Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle().NonLazy();
      Container.Bind<SpawnManager>().FromInstance(_spawnManager).AsSingle().NonLazy();
    }
  }
}