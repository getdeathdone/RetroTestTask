using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class AchievementController: object, IInitialize, IDeinitialize
  {
    private const float RicochetStrengthGainChance = 0.7f; // Шанс пополнения силы при убийстве рикошетным снарядом
    private const float RicochetHealthGainChance = 0.3f;   // Шанс пополнения здоровья при убийстве рикошетным снарядом
    private const int RicochetStrengthGain = 10;           // Количество силы, пополняемой при убийстве рикошетным снарядом
    private const float RicochetHealthGainPercentage = 0.5f; // Процент здоровья, пополняемого при убийстве рикошетным снарядом

    private readonly EnemyController _enemyController;
    private readonly PlayerController _playerController;

    [Inject]
    public AchievementController(EnemyController enemyController, PlayerController playerController)
    {
      _playerController = playerController;
      _enemyController = enemyController;
    }

    public void Initialize()
    {
      _playerController.Player.OnDeath += OnDied;
      
      foreach (var VARIABLE in _enemyController.Enemies)
      {
        VARIABLE.OnDeath += OnDied;
      }
      
      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      _playerController.Player.OnDeath -= OnDied;
      
      foreach (var VARIABLE in _enemyController.Enemies)
      {
        VARIABLE.OnDeath -= OnDied;
      }
      
      IsInitialized = false;
    }

    private void OnDied (DamageInfo damageInfo)
    {
      var hero = ((Health)damageInfo.Receiver).ComponentOwner;

      if (hero.Side == HeroSide.None)
      {
        Debug.LogWarning("HeroSide.None");
        return;
      }

      if (hero.Side == HeroSide.Player)
      {
        Deinitialize();
        return;
      }

      if (damageInfo.AttackType == AttackType.Ricochet)
      {
        RicochetKillBonus();
      }

      void RicochetKillBonus()
      {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= RicochetStrengthGainChance)
        {
          var attack = damageInfo.DamageDealer.GetAttachedComponent<Attack>();
          attack.AddStrength(RicochetStrengthGain);
        } else
        {
          var health = damageInfo.DamageDealer.GetAttachedComponent<Health>();
          float healthToRestore = health.MaxHealth * RicochetHealthGainPercentage;

          health.RestoreHealth((int)healthToRestore);
        }
      }
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}