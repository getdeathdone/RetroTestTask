using DefaultNamespace.Controller;
using DefaultNamespace.Handler;
using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameScene
{
  public class GameSceneInstaller : MonoInstaller
  {
    [SerializeField]
    private AreaManager _areaManager;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private SpawnHandler _spawnHandler;
    [SerializeField]
    private CameraManager _cameraManager;

    public override void InstallBindings()
    {
      Container.Bind<GameController>().AsSingle().NonLazy();
      Container.Bind<InputHandler>().AsSingle().NonLazy();

      Container.Bind<CameraManager>().FromInstance(_cameraManager).AsSingle().NonLazy();
      Container.Bind<AreaManager>().FromInstance(_areaManager).AsSingle().NonLazy();
      Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle().NonLazy();
      Container.Bind<SpawnHandler>().FromInstance(_spawnHandler).AsSingle().NonLazy();
    }
  }
}