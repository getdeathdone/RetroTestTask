using System.Collections.Generic;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;

namespace DefaultNamespace.Hero
{
  public class HeroPlayer : HeroBase
  {
    private InputManager _inputManager;
    public InputManager InputManager => _inputManager;

    public override HeroType Type => HeroType.Player;

    public override void SetInject (List<IInject> injects)
    {
      base.SetInject(injects);
      _inputManager = SetInject<InputManager>(injects);
    }
  }
}