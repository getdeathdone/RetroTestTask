namespace DefaultNamespace.Hero
{
  public class HeroEnemy : HeroBase
  {
    public override HeroSide Side => HeroSide.Enemy;

    public void DestroyEnemy()
    {
      Destroy(gameObject);
    }
  }
}