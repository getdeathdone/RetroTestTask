using System;

namespace DefaultNamespace
{
  public static class GameConstants
  {
    public static class Achievement
    {
      public const float RicochetStrengthGainChance = 0.7f;   // Шанс пополнения силы при убийстве рикошетным снарядом
      public const float RicochetHealthGainChance = 0.3f;     // Шанс пополнения здоровья при убийстве рикошетным снарядом
      public const int RicochetStrengthGain = 10;             // Количество силы, пополняемой при убийстве рикошетным снарядом
      public const float RicochetHealthGainPercentage = 0.5f; // Процент здоровья, пополняемого при убийстве рикошетным снарядом
      
      private const int RED_KILL_ACHIEVEMENT = 15;
      private const int BLUE_KILL_ACHIEVEMENT = 50;
      
      public static int CalculateStrengthAchievement(HeroType heroType)
      {
        return heroType switch
        {
          HeroType.EnemyRed => RED_KILL_ACHIEVEMENT,
          HeroType.EnemyBlue => BLUE_KILL_ACHIEVEMENT,
          _ => throw new ArgumentOutOfRangeException(nameof(heroType), heroType, null)
        };
      }
    }
    
    public static class Battle
    {
      public const int PLAYER_KILL_TO_WIN = 20;
    }
    
    public static class Enemy
    {
      public const int BLUE_ENEMY_RATIO = 1;
      public const int RED_ENEMY_RATIO = 4;
    
      public const int MAX_ENEMY_COUNT = 30;
      public const int START_ENEMY_COUNT = 5;
    
      public const float INITIAL_SPAWN_INTERVAL = 30f;
      public const float MIN_SPAWN_INTERVAL = 6.0f;
      public const float SPAWN_INTERVAL_DECREMENT = 2.0f;
    }
  }
}