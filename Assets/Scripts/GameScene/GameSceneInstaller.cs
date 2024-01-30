using DefaultNamespace.Controller;
using DefaultNamespace.Manager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.GameScene
{
  public class GameSceneInstaller : MonoInstaller
  {
    [SerializeField]
    private UIManager _uiManager;

    public override void InstallBindings()
    {
      Container.Bind<InputManager>().AsSingle().NonLazy();
      Container.Bind<GameController>().AsSingle().NonLazy();
      Container.Bind<BattleController>().AsSingle().NonLazy();
      
      Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle().NonLazy();
    }
  }
}