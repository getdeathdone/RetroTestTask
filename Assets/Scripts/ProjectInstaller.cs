using DefaultNamespace.Handler;
using DefaultNamespace.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
  public class ProjectInstaller : MonoInstaller
  {
    [SerializeField]
    private CoroutineHandler _coroutineHandler;
    [SerializeField]
    private GameConfig _gameConfig;

    public override void InstallBindings()
    {
      Container.Bind<CoroutineHandler>().FromInstance(_coroutineHandler).AsSingle().NonLazy();
      Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
    }
  }
}