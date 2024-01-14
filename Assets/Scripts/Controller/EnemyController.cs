using System.Collections.Generic;
using DefaultNamespace.Hero;

namespace DefaultNamespace.Controller
{
  public class EnemyController
  {
    private List<HeroEnemy> _enemies = new List<HeroEnemy>();

    public void AddEnemy (HeroEnemy enemy)
    {
      _enemies.Add(enemy);
    }
  }
}