using DefaultNamespace.Hero;

namespace DefaultNamespace.Controller
{
  public class PlayerController
  {
    private HeroPlayer _player;
    public HeroPlayer Player => _player;

    public void SetPlayer(HeroPlayer heroBase)
    {
      _player = heroBase;
    }
  }
}